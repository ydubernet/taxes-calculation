using PlusValuesFifo.Models;
using System.Collections.Generic;

namespace PlusValuesFifo.Services
{
    public interface IPlusValuesService
    {
        bool TryComputePlusValues(IEnumerable<IEvent> events, out IList<OutputEvent> outputs);
    }
}