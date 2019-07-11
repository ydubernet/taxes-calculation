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
            var facebookBuyEvent2 = new InputEvent("FB UW", BuySell.Buy, 2, 178.5m, new DateTime(2018, 7, 27, 15, 33, 00), 0.51m);
            var facebookBuyEvent1 = new InputEvent("FB UW", BuySell.Buy, 2, 164m, new DateTime(2018, 04, 16, 16, 49, 00), 0.51m);
            var spotifyBuyEvent1 = new InputEvent("SPOT", BuySell.Buy, 3, 147m, new DateTime(2018, 04, 16, 16, 26, 00), 0.51m);
            var spotifySellEvent1 = new InputEvent("SPOT", BuySell.Sell, 3, 114.28m, new DateTime(2018, 12, 21, 15, 33, 00), 0.51m);
            var spotifyBuyEvent2 = new InputEvent("SPOT", BuySell.Buy, 2, 111.67m, new DateTime(2018, 12, 26, 15, 30, 00), 0.51m);
            var facebookSellEvent1 = new InputEvent("FB UW", BuySell.Sell, 2, 125.99m, new DateTime(2018, 12, 26, 15, 30, 00), 0.51m);

        }
    }
}
