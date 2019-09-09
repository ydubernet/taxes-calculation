using PlusValuesFifo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlusValuesFifo.Models.Equities
{
    /// <summary>
    /// This helper intends to create an OutputEvent collection
    /// containing already existing output events (for instance would contain pmp, pv after computation)
    /// and to add it input events which won't have had any needed computation and won't then be in the outputEvents collection
    /// </summary>
    public static class OutputDataFormaterHelper
    {
        public static IList<IOutputEvent> ConcatAllEvents(IList<IInputEvent> inputEvents, IList<IOutputEvent> outputEvents, AssetType assetType)
        {   
            var inputEventsAsOutputCollection = new List<IOutputEvent>();
            foreach (var input in inputEvents)
            {
                if (assetType == AssetType.Equity)
                {
                    inputEventsAsOutputCollection.Add(new EquitiesOutputEvent(0, 0, input as IEquitiesInputEvent));
                }

                // :)
                //else
                //    inputEventsAsOutputCollection.Add(new CryptoOutputEvent(0,0,input as ICryptoInputEvent)):
            }

            return inputEventsAsOutputCollection.Union(outputEvents).OrderBy(e => e.Date).ToList();
        }
    }
}
