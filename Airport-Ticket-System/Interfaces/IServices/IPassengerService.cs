using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.Interfaces.IServices
{
    public interface IPassengerService
    {
        IEnumerable<Flight> SearchAvailableFlights(decimal? maxPrice = null,
                                                   string? departureCountry = null,
                                                   string? destinationCountry = null,
                                                   DateTime? departureDate = null,
                                                   string? departureAirport = null,
                                                   string? arrivalAirport = null,
                                                   string? flightClass = null);

        void BookFlight(string passengerId, string flightId, FlightClass flightClass);

        IEnumerable<Booking> ViewBookings(string passengerId);

        void CancelBooking(string passengerId, string bookingId);

        void ModifyBooking(string passengerId, string bookingId, string flightId,FlightClass newClass);
        public void PrintAllFlights();
    }

}
