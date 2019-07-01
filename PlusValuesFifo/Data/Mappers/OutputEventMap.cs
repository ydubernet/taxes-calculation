using CsvHelper.Configuration;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Data.Mappers
{
    public class OutputEventMap : ClassMap<OutputEvent>
    {
        public OutputEventMap()
        {
            Map(m => m.AssetName).Index(0);
            Map(m => m.Date).Index(1);
            Map(m => m.ActionEvent).Index(2);
            Map(m => m.Amount).Index(3);
            Map(m => m.Price).Index(4);
            Map(m => m.Fee).Index(5);
            Map(m => m.Pmp).Index(6);
            Map(m => m.PlusValue).Index(7);
            Map(m => m.AmountUsed).Ignore();
        }
    }
}
