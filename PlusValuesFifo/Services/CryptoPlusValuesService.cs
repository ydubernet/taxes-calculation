using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;

namespace PlusValuesFifo.Services
{
    public class CryptoPlusValuesService : IPlusValuesService
    {
        private readonly ILogger<CryptoPlusValuesService> _logger;

        public CryptoPlusValuesService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CryptoPlusValuesService>();
        }

        public IList<OutputEvent> ComputePlusValues(IEnumerable<IEvent> events)
        {
            throw new NotImplementedException();
        }
    }
}
