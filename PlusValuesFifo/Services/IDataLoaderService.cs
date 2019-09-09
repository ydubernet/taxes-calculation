using PlusValuesFifo.Models;
using System.Collections.Generic;

namespace PlusValuesFifo.Data
{
    public interface IDataLoaderService<T>
    {
        bool TryLoadData(string content, AssetType assetType);
        IEnumerable<T> GetEvents();
    }
}