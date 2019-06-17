using CsvHelper.Configuration;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Data.Mappers
{
    public class EventMap<T> : ClassMap<T> where T : IEvent
    {
        public EventMap()
        {
            AutoMap();
            Map(m => m.Fee).Name("Fee").Default(0.0m);
            Map(m => m.AmountUsed).Ignore();
        }
    }
}
