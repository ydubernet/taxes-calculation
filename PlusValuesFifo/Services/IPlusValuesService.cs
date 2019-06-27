using PlusValuesFifo.Models;
using System.Collections.Generic;

namespace PlusValuesFifo.Services
{
    public interface IPlusValuesService
    {
        IList<OutputEvent> ComputePlusValues(IEnumerable<IEvent> events);
    }
}