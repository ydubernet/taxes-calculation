using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Models
{
    public abstract class Event : IEvent
    {
        public Event(DateTime date, decimal amount, decimal price)
        {
            Amount = amount;
            Price = price;
            Date = date;
        }

        public abstract BuySell ActionEvent { get; }

        public decimal Amount { get; set; } // /!\
        public decimal Price { get; }
        public DateTime Date { get; }
    }
}
