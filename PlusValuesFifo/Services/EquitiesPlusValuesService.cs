using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;
using PlusValuesFifo.Models.Equities;
using System.Collections.Generic;
using System.Linq;

namespace PlusValuesFifo.Services
{
    /// <summary>
    /// Service computing the Equities Capital Gains of a given list of buy and sell events
    ///
    /// (c) 2019 - Yoann DUBERNET yoann [dot] dubernet [at] gmail [dot] com
    ///
    /// By "Service", we mean the service the contained code of this file implements
    ///
    /// SPECIFIC LICENCE APPLYING TO THIS SERVICE :
    /// - THIS SERVICE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
    ///   INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
    ///   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
    ///   WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF
    ///   OR IN CONNECTION WITH THE SERVICE OR THE USE OR OTHER DEALINGS IN THE SERVICE.
    /// - You can suggest improvements by opening a pull-request on https://github.com/ydubernet/taxes-calculation,
    ///   re-use this service for your own software(s) if and only if the software(s) using it is(are) free and open-sourced.
    /// - By reusing this service, you agree to quote its original author and all its contributors in a COPYRIGHT file
    ///   and to notify them of your usage.
    ///
    /// Disclamers :
    /// - This service computes Capital Gain for EQUITIES ONLY
    ///   It is based on notice2074 2018 French Taxes declaration.
    /// - Do not forget that taxes forms change every year, this service may not be up date when you use it
    /// - Do not forget that every situation is different and notice2074 applies to you
    ///   if and only if you are qualified as a physical person, not as a professional person or company
    /// - The author is not a taxes professional and this implementation is his sole understanding
    ///   of the declaration notice and he has not received any aproval from any taxes professional
    ///   regarding his implementation.
    /// - Always do your own checks regarding the computed results
    /// - The results returned by this service are NOT valid for crypto assets
    ///   given the 2019 crypto taxes computation formula
    /// - At the moment, this implementation has only been tested with Long Buy-Sell events.
    ///   It has not been tested with Short Sell-Buy strategies.
    /// </summary>
    public class EquitiesPlusValuesService : IPlusValuesService
    {
        private readonly ILogger<EquitiesPlusValuesService> _logger;

        public EquitiesPlusValuesService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EquitiesPlusValuesService>();
        }

        // TODO : For first version, since we don't support short selling, add a sanity check so we wouldn't be selling more than what we own

        public IEnumerable<IOutputEvent> ComputePlusValues(IEnumerable<IInputEvent> events)
        {
            var outputs = new List<IOutputEvent>();

            foreach (var assetEvents in events.GroupBy(e => e.AssetName))
            {
                var assetOutputs = ComputePlusValuesForEachAsset(assetEvents);
                outputs.AddRange(assetOutputs);
                _logger.LogInformation($"Done for asset : {assetEvents.Key}");
            }

            _logger.LogInformation("Done");

            return outputs;
        }

        private IList<IOutputEvent> ComputePlusValuesForEachAsset(IEnumerable<IInputEvent> events)
        {
            // Buy data
            List<EquitiesInputEvent> buyEvents = events.Where(e => e.ActionEvent == BuySell.Buy).OrderBy(e => e.Date).Cast<EquitiesInputEvent>().ToList();
            // Sell data
            List<EquitiesInputEvent> sellEvents = events.Where(e => e.ActionEvent == BuySell.Sell).OrderBy(e => e.Date).Cast<EquitiesInputEvent>().ToList();

            var outputs = new List<IOutputEvent>();

            foreach (var sellEvent in sellEvents)
            {
                _logger.LogDebug($"Sell event : Date : {sellEvent.Date}, Amount : {sellEvent.Amount}, Price : {sellEvent.Price}.");
                var previousBuyEvents = buyEvents.Where(e => e.Date <= sellEvent.Date)
                                                 .Where(e => e.AmountUsed < e.Amount) // Useless to keep buying events whose calculation has been all taken into account
                                                 .ToList();

                // Average buying price of ALL buying events prior to the current selling event
                decimal pmp = previousBuyEvents.Sum(be => (be.Amount - be.AmountUsed) * be.Price) / previousBuyEvents.Sum(be => (be.Amount - be.AmountUsed));
                // plus value for the selling event given average buying price
                decimal pv = (sellEvent.Price - pmp) * (sellEvent.Amount - sellEvent.AmountUsed);

                // store these infos for output
                outputs.Add(new EquitiesOutputEvent(pmp, pv, sellEvent));

                _logger.LogDebug($"Pmp : {pmp}, Pv : {pv}");

                // Appliance of the FIFO algorithm :
                foreach (var previousBuyEvent in previousBuyEvents)
                {
                    if (sellEvent.AmountUsed == sellEvent.Amount) // No need to check any other buy event if the current sellevent has already been 100% taken into account
                        break;

                    if (previousBuyEvent.AmountUsed + sellEvent.Amount - sellEvent.AmountUsed <= previousBuyEvent.Amount)
                    {
                        previousBuyEvent.AmountUsed += (sellEvent.Amount - sellEvent.AmountUsed);
                        sellEvent.AmountUsed = sellEvent.Amount;
                    }
                    else
                    {
                        sellEvent.AmountUsed += (previousBuyEvent.Amount - previousBuyEvent.AmountUsed);
                        previousBuyEvent.AmountUsed = previousBuyEvent.Amount;
                    }
                }
                _logger.LogDebug($"Done for events prior to {sellEvent.Date}");
            }
            return outputs;
        }
    }
}
