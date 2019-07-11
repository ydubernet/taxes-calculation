using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;
using PlusValuesFifo.Services;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace PlusValuesFifoUnitTests
{
    public class PlusValuesServiceTests
    {
        private readonly ILogger<PlusValuesService> _logger;

        public PlusValuesServiceTests()
        {
            _logger = new Mock<ILogger<PlusValuesService>>().Object;
        }

        [Fact]
        public void Test_That_Default_Notice_2074_Exemple_Computes_Correctly()
        {
            var buyingEvent1 = new InputEvent("X", BuySell.Buy, 100, 95, DateTime.UtcNow.AddYears(-5), 0);
            var buyingEvent2 = new InputEvent("X", BuySell.Buy, 200, 110, DateTime.UtcNow.AddYears(-3), 0);
            var sellingEvent = new InputEvent("X", BuySell.Sell, 150, 120, DateTime.UtcNow, 0);

            var allInputEvents = new List<InputEvent>() { buyingEvent1, buyingEvent2, sellingEvent };

            var plusValuesServices = new PlusValuesService(_logger);
            var outputEvents = plusValuesServices.ComputePlusValues(allInputEvents);

            Assert.NotNull(outputEvents);
            Assert.Equal(1, outputEvents.Count);
            Assert.NotNull(outputEvents.Single());
            Assert.Equal(105, outputEvents.Single().Pmp);
            Assert.Equal(2250, outputEvents.Single().PlusValue);
            Assert.Equal(100, buyingEvent1.AmountUsed);
            Assert.Equal(50, buyingEvent2.AmountUsed);
            Assert.Equal(150, sellingEvent.AmountUsed);
            Assert.Equal("X", outputEvents.Single().AssetName);
        }

        [Fact]
        public void Test_Simple_Case()
        {
            var buyEvent = new InputEvent("X", BuySell.Buy, 1, 42.12m, DateTime.Now.AddHours(-1), 0);
            var sellEvent = new InputEvent("X", BuySell.Sell, 1, 43.85m, DateTime.Now.AddMinutes(-1), 0);

            var allInputEvents = new List<InputEvent>() { buyEvent, sellEvent };

            var plusValuesServices = new PlusValuesService(_logger);
            var outputEvents = plusValuesServices.ComputePlusValues(allInputEvents);

            Assert.NotNull(outputEvents);
            Assert.Equal(1, outputEvents.Count);
            Assert.NotNull(outputEvents.Single());
            Assert.Equal(42.12m, outputEvents.Single().Pmp);
            Assert.Equal(43.85m - 42.12m, outputEvents.Single().PlusValue);
            Assert.Equal(1, buyEvent.AmountUsed);
            Assert.Equal(1, sellEvent.AmountUsed);
            Assert.Equal("X", outputEvents.Single().AssetName);
        }

        [Fact]
        public void Test_PlusValue_Service_Behaves_Correctly_With_Many_Assets_In_Input()
        {
            var facebookBuyEvent2 = new InputEvent("FB UW", BuySell.Buy, 2, 153.14m, new DateTime(2017, 7, 25), 0.51m);
            var facebookBuyEvent1 = new InputEvent("FB UW", BuySell.Buy, 2, 132.39m, new DateTime(2017, 4, 15), 0.51m);
            var spotifyBuyEvent1 = new InputEvent("SPOT", BuySell.Buy, 3, 118m + 2/3m, new DateTime(2017, 4, 14), 0.51m);
            var spotifySellEvent1 = new InputEvent("SPOT", BuySell.Sell, 3, 100.14666666667m, new DateTime(2017, 12, 1), 0.51m);
            var spotifyBuyEvent2 = new InputEvent("SPOT", BuySell.Buy, 2, 97.89m, new DateTime(2017, 12, 5), 0.51m);
            var facebookSellEvent1 = new InputEvent("FB UW", BuySell.Sell, 2, 110.765m, new DateTime(2017, 12, 5), 0.51m);

            var allInputEvents = new List<InputEvent>() { facebookBuyEvent2, facebookBuyEvent1, spotifyBuyEvent1, spotifySellEvent1, spotifyBuyEvent2, facebookSellEvent1 };

            var plusValuesServices = new PlusValuesService(_logger);
            var outputEvents = plusValuesServices.ComputePlusValues(allInputEvents);


            Assert.NotNull(outputEvents);
            Assert.Equal(2, outputEvents.Count);
            Assert.NotNull(outputEvents[0]);
            Assert.NotNull(outputEvents[1]);

            // Since we group events by their asset name facebook should be the first in the list
            var facebookOutputEvent = outputEvents[0];
            var spotifyOutputEvent = outputEvents[1];

            Assert.Equal("SPOT", spotifyOutputEvent.AssetName);
            Assert.Equal("FB UW", facebookOutputEvent.AssetName);

            Assert.Equal(142.765m, facebookOutputEvent.Pmp);
            Assert.Equal(-64m, facebookOutputEvent.PlusValue);
            Assert.Equal(2, facebookBuyEvent1.AmountUsed);
            Assert.Equal(0, facebookBuyEvent2.AmountUsed);
            Assert.Equal(2, facebookSellEvent1.AmountUsed);

            Assert.Equal(118 + 2 / 3m, spotifyOutputEvent.Pmp);
            Assert.Equal(-55.56m, spotifyOutputEvent.PlusValue, 3);
            Assert.Equal(3, spotifyBuyEvent1.AmountUsed);
            Assert.Equal(0, spotifyBuyEvent2.AmountUsed);
            Assert.Equal(3, spotifySellEvent1.AmountUsed);
        }

        [Fact]
        public void Test_PlusValuesService_With_Multiple_Buy_Sell_Without_Selling_Everything_At_Once()
        {

        }
    }
}
