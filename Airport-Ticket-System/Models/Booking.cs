namespace Airport_Ticket_System.Models
{
    public class Booking
    {
        public required string BookingId { get; set; }
        public required string PassengerId { get; set; }
        public required string FlightId { get; set; }
        public DateTime BookingDate { get; set; }
        public FlightClass Class { get; set; }
        public decimal Price { get; set; }
    }
}
