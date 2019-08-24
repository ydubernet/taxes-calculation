using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;
using PlusValuesFifo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.ServiceProviders
{
    public class PlusValuesServiceProvider : IPlusValuesServiceProvider
    {
        private readonly ILoggerFactory _loggerFactory;

        public PlusValuesServiceProvider(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            Init();
        }

        private IDictionary<AssetType, IPlusValuesService> _plusValuesServiceDictionary;

        private void Init()
        {
            _plusValuesServiceDictionary = new Dictionary<AssetType, IPlusValuesService>();
            _plusValuesServiceDictionary.Add(AssetType.Equity, new EquitiesPlusValuesService(_loggerFactory));
            _plusValuesServiceDictionary.Add(AssetType.CryptoCurrency, new CryptoPlusValuesService(_loggerFactory));
        }

        public IPlusValuesService GetPlusValuesService(AssetType assetType)
        {
            return _plusValuesServiceDictionary[assetType];
        }
    }
}
