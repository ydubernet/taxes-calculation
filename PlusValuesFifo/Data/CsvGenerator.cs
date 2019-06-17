using CsvHelper;
using PlusValuesFifo.Data.Mappers;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Data
{
    public class CsvGenerator<T> : IFileGenerator<T> where T : IEvent
    {

        public void GenerateOutputFile(string outputPath, IEnumerable<T> events)
        {
            using(var stringWriter = new StreamWriter(outputPath))
            using (var csvWriter = new CsvWriter(stringWriter))
            {
                csvWriter.Configuration.HasHeaderRecord = true;
                csvWriter.Configuration.Delimiter = ";";

                var classMapper = new EventMap<T>();
                csvWriter.Configuration.RegisterClassMap(classMapper);

                csvWriter.WriteRecords(events);
            }
        }
    }
}
