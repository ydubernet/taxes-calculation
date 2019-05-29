using PlusValuesFifo.Data;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Services
{
    public class PlusValuesService : IPlusValuesService
    {
        private IDataLoader _dataLoader;

        public PlusValuesService(IDataLoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        //public bool TryComputePlusValues()
        //{
        //    var orderedBuySellData = _dataLoader.LoadData()
        //                                        .OrderBy(d => d.Date);

        //    // From there, make it a FIFO list
        //    Queue<BuyEvent> fifoBuyData = null;
        //    Queue<BuyEvent> fifoSellData = null;

        //    fifoBuyData = new Queue<BuyEvent>(orderedBuySellData.Where(d => d.Action == BuySell.Buy));
        //    fifoSellData = new Queue<SellEvent>(orderedBuySellData.Where(d => d.Action == BuySell.Sell));

        //    // Each time we sell, we want to calculate plus value of the corresponding sold amount
        //    // (in a future version, we may be interested in calculating plusvalue of shorted assets

        //    // We unpool sell events
        //    foreach(var sellData in fifoSellData)
        //    {
        //        List<BuyEvent> plusValues = new List<BuyEvent>();
        //        BuyEvent buyData;
        //        do
        //        {
        //            buyData = fifoBuyData.Dequeue();
        //            plusValues.Add(buyData);
        //        }while(buyData.Date < sellData.Date)
 
        //         var plusValue = sellData.Amount * sellData.Price 
                
                    
                
        //    }

        //    // To export each time there is a sell event: PMP (prix moyen pondéré) and PV (PlusValue)
        //}

        public bool TryComputePlusValues()
        {
            // Load data
            if(!_dataLoader.TryLoadData())
            {
                return false;
            }

            // Buy data
            List<BuyEvent> buyEvents = _dataLoader.GetBuyEvents().OrderBy(e => e.Date).ToList();
            // Sell data
            List<SellEvent> sellEvents = _dataLoader.GetSellEvents().OrderBy(e => e.Date).ToList();

            IList<(decimal, decimal)> outputs = new List<(decimal Pmp, decimal Pv)>();


            foreach(var sellEvent in sellEvents)
            {
                Console.WriteLine($"Sell event : Date : {sellEvent.Date}, Amount : {sellEvent.Amount}, Price : {sellEvent.Price}.");
                var previousBuyEvents = buyEvents.Where(e => e.Date <= sellEvent.Date).ToList();

                // Average buying price of ALL buying events prior to the current selling event
                decimal pmp = previousBuyEvents.Sum(be => be.Amount * be.Price) / previousBuyEvents.Sum(be => be.Amount);
                // plus value for the selling event given average buying price
                decimal pv = (sellEvent.Price - pmp) * sellEvent.Amount;
                
                // store these infos for output
                outputs.Add((pmp, pv));

                Console.WriteLine($"Pmp : {pmp}, Pv : {pv}");

                // Appliance of the FIFO algorithm :
                foreach (var previousBuyEvent in previousBuyEvents)
                {
                    // BERK BERK BERK (effets de bord)
                    if (previousBuyEvent.Amount < sellEvent.Amount)
                    {
                        sellEvent.Amount -= previousBuyEvent.Amount;
                        previousBuyEvent.Amount = 0;
                        buyEvents.Remove(previousBuyEvent);
                    }
                    else
                    {
                        previousBuyEvent.Amount -= sellEvent.Amount;
                        break;
                    }
                }

                Console.WriteLine($"Done for events prior to {sellEvent.Date}");
            }

            Console.WriteLine("Done");
            return true;
        }
    }
}
