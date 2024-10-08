namespace Airport_Ticket_System.Exceptions.FlightExceptions
{
    public class FlightNotFoundException : Exception
    {
        public FlightNotFoundException(string flightId) : base($"Flight with ID {flightId} was not found.") { }
    }
}
