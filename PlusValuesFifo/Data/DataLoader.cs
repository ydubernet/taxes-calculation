using CsvHelper;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlusValuesFifo.Data
{
    public class DataLoader<T> : IDataLoader<T> where T : IEvent
    {
        private readonly IParser<T> _parser;
        private readonly string _inputPath;
        private readonly ILogger _logger;
        private List<T> _events;

        public DataLoader(IParser<T> parser, string inputPath, ILogger logger)
        {
            _parser = parser;
            _inputPath = inputPath;
            _logger = logger;
        }


        public bool TryLoadData()
        {
            try
            {
                _events = _parser.Parse(_inputPath).ToList();
            }
            catch (CsvHelperException ex)
            {
                _logger.LogError(ex, $"Exception whilst parsing Csv...");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unmanaged error... :(");
                throw;
            }
            return true;
        }

        public IEnumerable<T> GetEvents()
        {
            return _events;
        }
    }
}
