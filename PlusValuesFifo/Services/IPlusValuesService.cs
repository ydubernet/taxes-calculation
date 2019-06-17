using PlusValuesFifo.Models;
using System.Collections.Generic;

namespace PlusValuesFifo.Services
{
    public interface IPlusValuesService
    {
        bool TryComputePlusValues(out IList<OutputEvent> outputs);
    }
}