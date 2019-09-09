using PlusValuesFifo.Models;
using System.Collections.Generic;

namespace PlusValuesFifo.Data
{
    public interface IDataExporterService<T>
    {
        bool TryExportData(IEnumerable<T> events, AssetType assetType, out string content);
    }
}
