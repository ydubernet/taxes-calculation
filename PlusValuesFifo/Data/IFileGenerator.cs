using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Data
{
    public interface IFileGenerator<T>
    {
        void GenerateOutputFile(string outputPath, IEnumerable<T> events);
    }
}
