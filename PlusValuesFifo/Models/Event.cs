using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Models
{
    public class Event : IEvent
    {
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
