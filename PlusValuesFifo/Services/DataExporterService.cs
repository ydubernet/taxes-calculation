using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;
using PlusValuesFifo.ServiceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Data
{
    public class DataExporterService<T> : IDataExporterService<T> where T : IOutputEvent
    {
        private readonly IFileGenerator<T> _fileGenerator;
        private readonly ILogger<DataExporterService<T>> _logger;

        public DataExporterService(IFileGenerator<T> fileGenerator, ILoggerFactory loggerFactory)
        {
            _fileGenerator = fileGenerator;
            _logger = loggerFactory.CreateLogger<DataExporterService<T>>();
        }

        public bool TryExportData(IEnumerable<T> events, AssetType assetType, out string content)
        {
            try
            {
                content = _fileGenerator.GenerateOutputFile(events, assetType);
                return true;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception occured during data export");
                content = string.Empty;
                return false;
            }
        }
    }
}
