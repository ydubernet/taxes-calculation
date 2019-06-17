using System;

namespace PlusValuesFifo.Models
{
    public interface IEvent
    {
        BuySell ActionEvent { get; }
        decimal Amount { get; }
        decimal Price { get; }
        DateTime Date { get; }
        decimal Fee { get; }

        /// <summary>
        /// <see cref="InputEvent.AmountUsed"/>
        /// </summary>
        decimal AmountUsed { get; set; }
    }
}