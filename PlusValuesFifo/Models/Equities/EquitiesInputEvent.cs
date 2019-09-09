using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Models.Equities
{
    public class EquitiesInputEvent : IEquitiesInputEvent, IInputEvent
    {
        /// <summary>
        /// Empty ctor for being able decode a csv input file containing a list of input events
        /// </summary>
        public EquitiesInputEvent() { }


        /// <summary>
        /// Ctor for test purposes
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="actionEvent"></param>
        /// <param name="amount"></param>
        /// <param name="price"></param>
        /// <param name="date"></param>
        /// <param name="fee"></param>
        public EquitiesInputEvent(string assetName,
                          BuySell actionEvent,
                          decimal amount,
                          decimal price,
                          DateTime date,
                          decimal fee)
        {
            AssetName = assetName;
            ActionEvent = actionEvent;
            Amount = amount;
            Price = price;
            Date = date;
            Fee = fee;
        }

        public string AssetName { get; set; }

        public BuySell ActionEvent { get; set; }

        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public decimal Fee { get; set; }

        /// <summary>
        /// Technical property for avoiding side calculations effects over the field Amount
        /// Should be the only one to have a seter in the interface
        /// </summary>
        public decimal AmountUsed { get; set; }
    }
}
