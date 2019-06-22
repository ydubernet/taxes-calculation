using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Data
{
    public interface IDataExporterService<T>
    {
        bool TryExportData(IEnumerable<T> events, out string content);
    }
}
