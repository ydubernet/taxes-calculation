using System;

namespace PlusValuesFifo.Models.Equities
{
    public class EquitiesOutputEvent : IEquitiesOutputEvent
    {
        public EquitiesOutputEvent(decimal pmp, decimal pv, IEquitiesInputEvent inputEvent)
        {
            Pmp = pmp;
            PlusValue = pv;

            AssetName = inputEvent.AssetName;
            ActionEvent = inputEvent.ActionEvent;
            Amount = inputEvent.Amount;
            Price = inputEvent.Price;
            Date = inputEvent.Date;
            Fee = inputEvent.Fee;
        }

        // CsvHelper generates the CSV output accorging to the bellow order
        public string AssetName { get; }

        public decimal Pmp { get; }
        public decimal PlusValue { get; }

        public BuySell ActionEvent { get; }
        public decimal Amount { get; }
        public decimal Price { get; }
        public DateTime Date { get; }
        public decimal Fee { get; }

        public decimal AmountUsed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
