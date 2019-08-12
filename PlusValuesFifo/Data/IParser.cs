using CsvHelper.Configuration;
using System.Collections.Generic;

namespace PlusValuesFifo.Data
{
    public interface IParser<T>
    {
        IReadOnlyCollection<T> Parse(string inputPath, ClassMap<T> classMapper);
    }
}