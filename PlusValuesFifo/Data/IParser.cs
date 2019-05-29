using PlusValuesFifo.Models;
using System.Collections.Generic;

namespace PlusValuesFifo.Data
{
    public interface IParser
    {
        IReadOnlyCollection<Event> Parse(string inputPath);
    }
}