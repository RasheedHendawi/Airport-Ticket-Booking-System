namespace Airport_Ticket_System.Models
{
    public class Flight
    {
        public required string FlightId { get; set; }
        [Validation("Free Text", "Required")]
        public required string DepartureCountry { get; set; }
        [Validation("Free Text", "Required")]
        public required string DestinationCountry { get; set; }
        [Validation("DateTime", "Required, Allowed Range (today → future)")]
        public DateTime DepartureDate { get; set; }
        [Validation("Free Text", "Required")]
        public required string DepartureAirport { get; set; }
        [Validation("Free Text", "Required")]
        public required string ArrivalAirport { get; set; }
        [Validation("Decimal", "Required, Must be greater than 0")]
        public decimal Price { get; set; }          
    }
}
