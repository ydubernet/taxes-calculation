using CsvHelper;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;
using PlusValuesFifo.ServiceProviders;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PlusValuesFifo.Data
{
    public class CsvGenerator<T> : IFileGenerator<T>
    {
        private readonly ILogger _logger;
        private readonly IMapProvider<T> _mapProvider;

        public CsvGenerator(ILoggerProvider loggerProvider, IMapProvider<T> mapProvider)
        {
            _mapProvider = mapProvider;
            _logger = loggerProvider.CreateLogger("CsvGenerator");
        }

        public string GenerateOutputFile(IEnumerable<T> events, AssetType assetType)
        {
            var mapper = _mapProvider.GetMap(assetType, EventType.Output);
            _logger.LogInformation($"Starting content generation with {mapper.GetType().Name} kind.");

            StringBuilder csvResultBuilder = new StringBuilder();
            using (var stringWriter = new StringWriter(csvResultBuilder))
            using (var csvWriter = new CsvWriter(stringWriter))
            {
                csvWriter.Configuration.HasHeaderRecord = true;
                csvWriter.Configuration.Delimiter = ";";

                csvWriter.Configuration.RegisterClassMap(mapper);

                csvWriter.WriteRecords(events);
            }

            return csvResultBuilder.ToString();
        }
    }
}
