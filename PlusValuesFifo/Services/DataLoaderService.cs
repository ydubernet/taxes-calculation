using CsvHelper;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;
using PlusValuesFifo.ServiceProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlusValuesFifo.Data
{
    public class DataLoaderService<T> : IDataLoaderService<T> where T : IInputEvent
    {
        private readonly IParser<T> _parser;
        private readonly ILogger<DataLoaderService<T>> _logger;
        private List<T> _events;

        public DataLoaderService(IParser<T> parser, ILoggerFactory loggerFactory)
        {
            _parser = parser;
            _logger = loggerFactory.CreateLogger<DataLoaderService<T>>();
        }


        public bool TryLoadData(string content, AssetType assetType)
        {
            try
            {
                _events = _parser.Parse(content, assetType).ToList();
            }
            catch (CsvHelperException ex)
            {
                _logger.LogError(ex, $"Exception whilst parsing Csv...");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unmanaged error... That's all we know... :(");
                throw ex;
            }
            return true;
        }

        public IEnumerable<T> GetEvents()
        {
            return _events;
        }
    }
}
