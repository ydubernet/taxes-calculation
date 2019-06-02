using CsvHelper;
using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlusValuesFifo.Data
{
    public class DataLoader : IDataLoader
    {
        private readonly IParser _parser;
        private readonly string _inputPath;
        private List<Event> _events;

        public DataLoader(IParser parser, string inputPath)
        {
            _parser = parser;
            _inputPath = inputPath;
        }


        public bool TryLoadData()
        {
            try
            {
                _events = _parser.Parse(_inputPath).ToList();
            }
            catch (CsvHelperException ex)
            {
                Console.WriteLine($"Exception whilst parsing Csv : {ex.Message}");
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        public IEnumerable<IEvent> GetBuyEvents()
        {
            foreach (var ev in _events)
                if (ev.ActionEvent == BuySell.Buy)
                    yield return ev;
        }


        public IEnumerable<IEvent> GetSellEvents()
        {
            foreach (var ev in _events)
                if (ev.ActionEvent == BuySell.Sell)
                    yield return ev;
        }
    }
}
