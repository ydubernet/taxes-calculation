using CsvHelper;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlusValuesFifo.Data
{
    public class DataLoaderService<T> : IDataLoaderService<T> where T : IEvent
    {
        private readonly IParser<T> _parser;
        private readonly ILogger<DataLoaderService<T>> _logger;
        private List<T> _events;

        public DataLoaderService(IParser<T> parser, ILogger<DataLoaderService<T>> logger)
        {
            _parser = parser;
            _logger = logger;
        }


        public bool TryLoadData(string content)
        {
            try
            {
                _events = _parser.Parse(content).ToList();
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
