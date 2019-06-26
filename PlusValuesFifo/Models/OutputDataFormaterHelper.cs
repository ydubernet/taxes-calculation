using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Services
{
    /// <summary>
    /// This service intends to create an OutputEvent collection
    /// containing already existing output events (for instance would contain pmp, pv after computation)
    /// and to add it input events which won't have had any needed computation and won't then be in the outputEvents collection
    /// </summary>
    public static class OutputDataFormaterHelper
    {
        public static IList<OutputEvent> ConcatAllEvents(IList<InputEvent> inputEvents, IList<OutputEvent> outputEvents)
        {
            var inputEventsAsOutputCollection = new List<OutputEvent>();
            foreach (var input in inputEvents)
            {
                inputEventsAsOutputCollection.Add(new OutputEvent(0, 0, input));
            }

            return inputEventsAsOutputCollection.Union(outputEvents).OrderBy(e => e.Date).ToList();
        }
    }
}
