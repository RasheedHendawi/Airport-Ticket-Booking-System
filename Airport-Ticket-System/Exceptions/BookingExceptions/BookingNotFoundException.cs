namespace Airport_Ticket_System.Exceptions.BookingExceptions
{
    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException(string bookingId)
            : base($"Booking with ID {bookingId} not found.")
        {
        }
    }
}
