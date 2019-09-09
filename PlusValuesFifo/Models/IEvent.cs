using System;

namespace PlusValuesFifo.Models
{
    public interface IEvent
    {
        string AssetName { get; }
        BuySell ActionEvent { get; }
        decimal Amount { get; }
        decimal Price { get; }
        DateTime Date { get; }
        decimal Fee { get; }

        // string Currency { get; } // TODO : Add currency management in the future
    }

    public interface IInputEvent : IEvent
    { }

    public interface IOutputEvent : IEvent
    {
        decimal PlusValue { get; }
    }
}