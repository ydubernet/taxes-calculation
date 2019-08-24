using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Data
{
    public class DataExporterService<T> : IDataExporterService<T> where T : IEvent
    {
        private readonly IFileGenerator<T> _fileGenerator;
        private readonly ILogger<DataLoaderService<T>> _logger;

        public DataExporterService(IFileGenerator<T> fileGenerator, ILoggerFactory loggerFactory)
        {
            _fileGenerator = fileGenerator;
            _logger = loggerFactory.CreateLogger<DataLoaderService<T>>();
        }

        public bool TryExportData(IEnumerable<T> events, out string content)
        {
            try
            {
                content = _fileGenerator.GenerateOutputFile(events);
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
