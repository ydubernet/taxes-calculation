using PlusValuesFifo.Models;
using System.Collections.Generic;

namespace PlusValuesFifo.Data
{
    public interface IParser<T>
    {
        IReadOnlyCollection<T> Parse(string inputPath, AssetType assetType);
    }
}