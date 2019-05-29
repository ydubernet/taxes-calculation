using System;

namespace PlusValuesFifo.Models
{
    public interface IEvent
    {
        decimal Amount { get; }
        decimal Price { get; }
        DateTime Date { get; }
    }
}