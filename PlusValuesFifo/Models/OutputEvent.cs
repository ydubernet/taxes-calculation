﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Models
{
    public class OutputEvent : IEvent
    {
        public OutputEvent(decimal pmp, decimal pv, IEvent inputEvent)
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
