using CsvHelper;
using CsvHelper.Configuration;
using PlusValuesFifo.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlusValuesFifo.Data
{
    public class EventMap : ClassMap<Event>
    {
        public EventMap()
        {
            AutoMap();
            Map(m => m.Fee).Name("Fee").Default(0.0m);
            Map(m => m.AmountUsed).Ignore();
        }
    }

    public class CsvParser : IParser
    {

        public IReadOnlyCollection<Event> Parse(string inputPath)
        {
            using (var stringReader = new StreamReader(inputPath))
            using (var csvReader = new CsvReader(stringReader))
            {
                csvReader.Configuration.HasHeaderRecord = true;
                csvReader.Configuration.Delimiter = ";";
                csvReader.Configuration.IgnoreBlankLines = true;

                var classMapper = new EventMap();
                csvReader.Configuration.RegisterClassMap(classMapper);

                return csvReader.GetRecords<Event>().ToList();
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
