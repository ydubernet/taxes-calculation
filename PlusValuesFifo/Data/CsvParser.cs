using CsvHelper;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;
using PlusValuesFifo.ServiceProviders;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlusValuesFifo.Data
{
    public class CsvParser<T> : IParser<T>
    {
        private readonly ILogger _logger;
        private readonly IMapProvider<T> _mapProvider;

        public CsvParser(ILoggerProvider loggerProvider, IMapProvider<T> mapProvider)
        {
            _logger = loggerProvider.CreateLogger("CsvParser");
            _mapProvider = mapProvider;
        }


        public IReadOnlyCollection<T> Parse(string content, AssetType assetType)
        {
            var classMapper = _mapProvider.GetMap(assetType, EventType.Input);
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
