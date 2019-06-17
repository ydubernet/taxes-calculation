using CsvHelper;
using PlusValuesFifo.Data.Mappers;
using PlusValuesFifo.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlusValuesFifo.Data
{
    public class CsvParser<T> : IParser<T> where T : IEvent
    {

        public IReadOnlyCollection<T> Parse(string inputPath)
        {
            using (var stringReader = new StreamReader(inputPath))
            using (var csvReader = new CsvReader(stringReader))
            {
                csvReader.Configuration.HasHeaderRecord = true;
                csvReader.Configuration.Delimiter = ";";
                csvReader.Configuration.IgnoreBlankLines = true;

                var classMapper = new EventMap<T>();
                csvReader.Configuration.RegisterClassMap(classMapper);

                return csvReader.GetRecords<T>().ToList();
            }
        }


        //public IReadOnlyCollection<Event> ParseMock(string inputPath)
        //{
        //    // TODO : Implement it (CsvParser Msft library)
        //    // ActionDate;ActionEvent;ActionAmount;ActionPrice

        //    return new List<Event>()
        //    {
        //        new BuyEvent(new DateTime(2018,01,01), 12, 4),
        //        new BuyEvent(new DateTime(2018,02,01), 3, 6),
        //        new SellEvent(new DateTime(2018,02,02), 5, 8),
        //        new BuyEvent(new DateTime(2018,10,31), 4, 7),
        //        new SellEvent(new DateTime(2018,11,03), 13, 10),
        //        new SellEvent(new DateTime(2018,12,31), 1, 14)
        //    };
        //    //throw new NotImplementedException();
        //}
    }
}
