using PlusValuesFifo.Models;
using System.Collections.Generic;

namespace PlusValuesFifo.Data
{
    public interface IDataLoader
    {
        bool TryLoadData();
        IEnumerable<BuyEvent> GetBuyEvents();
        IEnumerable<SellEvent> GetSellEvents();
    }
}