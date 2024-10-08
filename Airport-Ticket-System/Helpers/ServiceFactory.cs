using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Interfaces.IServices;
using Airport_Ticket_System.Repositories;
using Airport_Ticket_System.Services;

public static class ServiceFactory
{
    public static (IPassengerService PassengerService, IFlightService FlightService, IBookingService BookingService) CreateServices()
    {
        IBookingRepository bookingRepository = new BookingRepository();
        IFlightRepository flightRepository = new FlightRepository();
        IPassengerRepository passengerRepository = new PassengerRepository();

        IFlightService flightService = new FlightService(flightRepository);
        IBookingService bookingService = new BookingService(bookingRepository, flightRepository);
        IPassengerService passengerService = new PassengerService(flightRepository, bookingRepository);

        return (passengerService, flightService, bookingService);
    }
}
