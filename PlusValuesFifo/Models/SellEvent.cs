using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Models
{
    public class SellEvent : Event
    {
        public SellEvent(DateTime date, decimal amout, decimal price)
            : base(date, amout, price)
        {
        }

        public override BuySell ActionEvent => BuySell.Sell;

        // In a second version, could handle :
        public string Currency => "EUR";
    }
}
