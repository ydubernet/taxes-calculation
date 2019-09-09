using CsvHelper.Configuration;
using PlusValuesFifo.Data.Mappers;
using PlusValuesFifo.Models;
using PlusValuesFifo.Models.Equities;

namespace PlusValuesFifo.ServiceProviders
{
    public class ClassMapProvider<T> : IMapProvider<T> where T : IEquitiesInputEvent, IEquitiesOutputEvent
    {
        public ClassMap<T> GetMap(AssetType assetType, EventType eventType)
        {
            if (assetType == AssetType.Equity && eventType == EventType.Input)
            {
                return new EquitiesInputEventMap<T>();
            }
            if(assetType == AssetType.Equity && eventType == EventType.Output)
            {
                return new EquitiesOutputEventMap<T>();
            }
            if(assetType == AssetType.CryptoCurrency && eventType == EventType.Input)
            {

                
            }
            if (assetType == AssetType.CryptoCurrency && eventType == EventType.Output)
            {
            }
            return null;

        }
    }
}
