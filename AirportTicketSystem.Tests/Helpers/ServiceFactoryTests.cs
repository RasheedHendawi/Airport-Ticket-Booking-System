using Airport_Ticket_System.Helpers;

namespace AirportTicketSystem.Tests.Helpers
{
    public class ServiceFactoryTests
    {
        [Fact]
        public void CreateServices_ShouldReturnValidServices()
        {
            var (passengerService, flightService, bookingService) = ServiceFactory.CreateServices();

            Assert.NotNull(passengerService);
            Assert.NotNull(flightService);
            Assert.NotNull(bookingService);
        }
    }
}
