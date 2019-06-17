using PlusValuesFifo.Models;
using System.Collections.Generic;

namespace PlusValuesFifo.Data
{
    public interface IDataLoader<T>
    {
        bool TryLoadData();
        IEnumerable<T> GetEvents();
    }
}