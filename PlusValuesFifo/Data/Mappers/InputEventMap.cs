using CsvHelper.Configuration;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Data.Mappers
{
    public class InputEventMap<T> : ClassMap<T> where T : IEvent
    {
        public InputEventMap()
        {
            AutoMap();
            Map(m => m.AssetName).Name("Name");
            Map(m => m.Fee).Name("Fee").Default(0.0m);
            Map(m => m.AmountUsed).Ignore();
        }
    }
}
