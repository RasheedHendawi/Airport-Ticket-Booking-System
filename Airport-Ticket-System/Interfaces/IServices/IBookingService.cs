using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.Interfaces.IServices
{
    public interface IBookingService
    {
        IEnumerable<Booking> GetBookingsByPassengerId(string passengerId);
        void CancelBooking(string bookingId);
        void ModifyBooking(string bookingId, string newFlightId, FlightClass flightClass);
        void PrintBookings(IEnumerable<Booking> bookgins);
        public IEnumerable<Booking> FilterBookings(string? flightId = null,
                                                   decimal? maxPrice = null,
                                                   string? departureCountry = null,
                                                   string? destinationCountry = null,
                                                   DateTime? departureDate = null,
                                                   string? departureAirport = null,
                                                   string? arrivalAirport = null,
                                                   string? passengerId = null,
                                                   string? flightClass = null);
    }
}
