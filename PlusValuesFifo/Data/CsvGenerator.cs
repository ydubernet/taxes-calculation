using CsvHelper;
using PlusValuesFifo.Data.Mappers;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlusValuesFifo.Data
{
    //// It could be cool to have T where T : ClassMap to have different csv mappers.
    //// and yet to keep the OutputEvent class as a normal input of GenerateOutputFile
    //// since it doesn't need to have different kinds of IEvent
    public class CsvGenerator<T> : IFileGenerator<T> where T : OutputEvent
    {
        // TOOD : Add a logger

        public string GenerateOutputFile(IEnumerable<T> events)
        {
            StringBuilder csvResultBuilder = new StringBuilder();
            using(var stringWriter = new StringWriter(csvResultBuilder))
            using (var csvWriter = new CsvWriter(stringWriter))
            {
                csvWriter.Configuration.HasHeaderRecord = true;
                csvWriter.Configuration.Delimiter = ";";

                var classMapper = new OutputEventMap();
                csvWriter.Configuration.RegisterClassMap(classMapper);

                csvWriter.WriteRecords(events);
            }

            return csvResultBuilder.ToString();
        }
    }
}
