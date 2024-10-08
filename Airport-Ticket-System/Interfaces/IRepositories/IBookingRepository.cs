using Airport_Ticket_System.Models;


namespace Airport_Ticket_System.Interfaces.IRepositories
{
    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAllBookings();
        Booking GetBookingById(string bookingId);
        void AddBooking(Booking booking);
        void RemoveBooking(string bookingId);
        void UpdateBooking(Booking booking);
        IEnumerable<Booking> GetBookingByPassengerId(string passengerId);
    }
}
