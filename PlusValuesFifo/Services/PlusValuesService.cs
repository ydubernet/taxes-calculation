﻿using PlusValuesFifo.Data;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlusValuesFifo.Services
{
    public class PlusValuesService : IPlusValuesService
    {
        private IDataLoader _dataLoader;

        public PlusValuesService(IDataLoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public bool TryComputePlusValues()
        {
            // Load data
            if (!_dataLoader.TryLoadData())
            {
                return false;
            }

            // Buy data
            List<IEvent> buyEvents = _dataLoader.GetBuyEvents().OrderBy(e => e.Date).ToList();
            // Sell data
            List<IEvent> sellEvents = _dataLoader.GetSellEvents().OrderBy(e => e.Date).ToList();

            IList<(decimal, decimal)> outputs = new List<(decimal Pmp, decimal Pv)>();


            foreach (var sellEvent in sellEvents)
            {
                Console.WriteLine($"Sell event : Date : {sellEvent.Date}, Amount : {sellEvent.Amount}, Price : {sellEvent.Price}.");
                var previousBuyEvents = buyEvents.Where(e => e.Date <= sellEvent.Date)
                                                 .Where(e => e.AmountUsed < e.Amount) // Useless to keep buying events whose calculation has been all taken into account
                                                 .ToList();

                // Average buying price of ALL buying events prior to the current selling event
                decimal pmp = previousBuyEvents.Sum(be => (be.Amount - be.AmountUsed) * be.Price) / previousBuyEvents.Sum(be => (be.Amount - be.AmountUsed));
                // plus value for the selling event given average buying price
                decimal pv = (sellEvent.Price - pmp) * (sellEvent.Amount - sellEvent.AmountUsed);

                // store these infos for output
                outputs.Add((pmp, pv));

                Console.WriteLine($"Pmp : {pmp}, Pv : {pv}");

                // Appliance of the FIFO algorithm :
                foreach (var previousBuyEvent in previousBuyEvents)
                {
                    if (sellEvent.AmountUsed == sellEvent.Amount) // No need to check any other buy event if the current sellevent has already been 100% taken into account
                        break;

                    if (previousBuyEvent.AmountUsed + sellEvent.Amount - sellEvent.AmountUsed <= previousBuyEvent.Amount)
                    {
                        previousBuyEvent.AmountUsed += (sellEvent.Amount - sellEvent.AmountUsed);
                        sellEvent.AmountUsed = sellEvent.Amount;
                    }
                    else //if(previousBuyEvent.AmountUsed + sellEvent.Amount - sellEvent.AmountUsed > previousBuyEvent.Amount)
                    {
                        sellEvent.AmountUsed += (previousBuyEvent.Amount - previousBuyEvent.AmountUsed);
                        previousBuyEvent.AmountUsed = previousBuyEvent.Amount;
                    }
                }

                Console.WriteLine($"Done for events prior to {sellEvent.Date}");
            }

            Console.WriteLine("Done");
            return true;
        }
    }
}
