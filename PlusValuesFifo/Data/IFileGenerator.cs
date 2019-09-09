using PlusValuesFifo.Models;
using System.Collections.Generic;

namespace PlusValuesFifo.Data
{
    public interface IFileGenerator<T>
    {
        string GenerateOutputFile(IEnumerable<T> events, AssetType assetType);
    }
}
