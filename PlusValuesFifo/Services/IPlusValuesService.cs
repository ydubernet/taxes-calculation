using PlusValuesFifo.Models;
using System.Collections.Generic;

namespace PlusValuesFifo.Services
{
    public interface IPlusValuesService
    {
        IEnumerable<IOutputEvent> ComputePlusValues(IEnumerable<IInputEvent> events);
    }
}