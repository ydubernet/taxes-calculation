using CsvHelper.Configuration;
using PlusValuesFifo.Models;
using PlusValuesFifo.Models.Equities;

namespace PlusValuesFifo.Data.Mappers
{
    public class EquitiesOutputEventMap<T> : ClassMap<T> where T : IEquitiesOutputEvent
    {
        public EquitiesOutputEventMap()
        {
            Map(m => m.AssetName).Index(0);
            Map(m => m.Date).Index(1);
            Map(m => m.ActionEvent).Index(2);
            Map(m => m.Amount).Index(3);
            Map(m => m.Price).Index(4);
            Map(m => m.Fee).Index(5);
            Map(m => m.Pmp).Index(6);
            Map(m => m.PlusValue).Index(7);
        }
    }
}
