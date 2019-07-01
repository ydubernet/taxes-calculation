using CsvHelper;
using PlusValuesFifo.Data.Mappers;
using PlusValuesFifo.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlusValuesFifo.Data
{
    public class CsvParser<T> : IParser<T> where T : InputEvent
    {
        // TODO : Add a logger

        public IReadOnlyCollection<T> Parse(string content)
        {
            using (var stringReader = new StringReader(content))
            using (var csvReader = new CsvReader(stringReader))
            {
                csvReader.Configuration.HasHeaderRecord = true;
                csvReader.Configuration.Delimiter = ";";
                csvReader.Configuration.IgnoreBlankLines = true;

                var classMapper = new InputEventMap();
                csvReader.Configuration.RegisterClassMap(classMapper);

                return csvReader.GetRecords<T>().ToList();
            }
        }
    }
}
