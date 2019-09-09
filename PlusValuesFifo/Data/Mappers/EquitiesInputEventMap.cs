using CsvHelper.Configuration;
using PlusValuesFifo.Models;
using PlusValuesFifo.Models.Equities;

namespace PlusValuesFifo.Data.Mappers
{
    public class EquitiesInputEventMap<T> : ClassMap<T> where T : IEquitiesInputEvent
    {
        public EquitiesInputEventMap()
        {
            AutoMap();
            Map(m => m.AssetName).Name("Name");
            Map(m => m.Fee).Name("Fee").Default(0.0m);
            Map(m => m.AmountUsed).Ignore();
        }
    }
}
