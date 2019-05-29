using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            _events = _parser.Parse(_inputPath).ToList();
            return true;
        }

        public IEnumerable<BuyEvent> GetBuyEvents()
        {
            foreach (var ev in _events)
                if (ev is BuyEvent)
                    yield return ev as BuyEvent;
        }


        public IEnumerable<SellEvent> GetSellEvents()
        {
            foreach (var ev in _events)
                if (ev is SellEvent)
                    yield return ev as SellEvent;
        }
    }
}
