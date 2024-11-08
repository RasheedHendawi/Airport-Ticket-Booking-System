using Xunit;
using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Models;

namespace AirportTicketSystem.Tests.Helpers
{
    public class FlightClassHelperTests
    {
        [Theory]
        [InlineData(100, FlightClass.Economy, 102)]
        [InlineData(100, FlightClass.Business, 104)]
        [InlineData(100, FlightClass.FirstClass, 107)]
        public void GetFlightPrice_ShouldReturnCorrectPrice(decimal basePrice, FlightClass flightClass, decimal expectedPrice)
        {
            var actualPrice = FlightClassHelper.GetFlightPrice(basePrice, flightClass);

            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GetFlightPrice_ShouldReturnBasePriceForInvalidClass()
        {
            decimal basePrice = 100;

            var actualPrice = FlightClassHelper.GetFlightPrice(basePrice, (FlightClass)99);

            Assert.Equal(basePrice, actualPrice);
        }
    }
}
