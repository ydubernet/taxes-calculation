using CsvHelper.Configuration;
using PlusValuesFifo.Models;

namespace PlusValuesFifo.ServiceProviders
{
    public interface IMapProvider<T>
    {
        ClassMap<T> GetMap(AssetType assetType, EventType eventType);
    }
}