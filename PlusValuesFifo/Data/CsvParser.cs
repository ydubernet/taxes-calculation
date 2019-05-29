using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlusValuesFifo.Models;

namespace PlusValuesFifo.Data
{
    public class CsvParser : IParser
    {
        public IReadOnlyCollection<Event> Parse(string inputPath)
        {
            // TODO : Implement it (CsvParser Msft library)
            // ActionDate;ActionEvent;ActionAmount;ActionPrice

            return new List<Event>()
            {
                new BuyEvent(new DateTime(2018,01,01), 12, 4),
                new BuyEvent(new DateTime(2018,02,01), 3, 6),
                new SellEvent(new DateTime(2018,02,02), 5, 8),
                new BuyEvent(new DateTime(2018,10,31), 4, 7),
                new SellEvent(new DateTime(2018,11,03), 13, 10),
                new SellEvent(new DateTime(2018,12,31), 1, 14)
            };
            //throw new NotImplementedException();
        }
    }
}
