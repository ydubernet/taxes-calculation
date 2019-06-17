using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Data
{
    public class DataExporter<T> : IDataExporter<T> where T : IEvent
    {
        private readonly IFileGenerator<T> _fileGenerator;
        private readonly string _outputPath;
        private readonly ILogger _logger;

        public DataExporter(IFileGenerator<T> fileGenerator, string outputPath, ILogger logger)
        {
            _fileGenerator = fileGenerator;
            _outputPath = outputPath;
            _logger = logger;
        }

        public bool TryExportData(IEnumerable<T> events)
        {
            try
            {
                _fileGenerator.GenerateOutputFile(_outputPath, events);
                return true;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception occured during data export");
                return false;
            }
        }
    }
}
