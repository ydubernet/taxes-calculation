using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Data.Mappers;
using PlusValuesFifo.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlusValuesFifo.Data
{
    public class CsvParser<T> : IParser<T> where T : InputEvent
    {
        private readonly ILogger<CsvParser<T>> _logger;

        public CsvParser(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CsvParser<T>>();
        }


        public IReadOnlyCollection<T> Parse(string content, ClassMap<T> classMapper)
        {
            _logger.LogInformation($"Starting content parsing with {classMapper.GetType().Name} kind.");

            using (var stringReader = new StringReader(content))
            using (var csvReader = new CsvReader(stringReader))
            {
                csvReader.Configuration.HasHeaderRecord = true;
                csvReader.Configuration.Delimiter = ";";
                csvReader.Configuration.IgnoreBlankLines = true;

                csvReader.Configuration.RegisterClassMap(classMapper);

                return csvReader.GetRecords<T>().ToList();
            }
        }
    }
}
