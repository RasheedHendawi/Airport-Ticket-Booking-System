using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Interfaces.IServices;
using Airport_Ticket_System.RepositoriesHandler;
using Airport_Ticket_System.Services;

namespace Airport_Ticket_System.Helpers
{
    public static class ServiceFactory
    {
        public static (IPassengerService PassengerService, IFlightService FlightService, IBookingService BookingService) CreateServices()
        {
            IBookingRepository bookingRepository = new BookingRepository("bookings.csv");
            IFlightRepository flightRepository = new FlightRepository("flights.csv");

            IFlightService flightService = new FlightService(flightRepository);
            IBookingService bookingService = new BookingService(bookingRepository, flightRepository);
            IPassengerService passengerService = new PassengerService(flightRepository, bookingRepository);

            return (passengerService, flightService, bookingService);
        }
    }
}//dotnet add package Microsoft.Extensions.DependencyInjection what about using this ?!!! TODO