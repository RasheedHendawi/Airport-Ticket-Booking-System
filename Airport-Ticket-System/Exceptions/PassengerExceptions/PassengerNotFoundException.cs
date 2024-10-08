namespace Airport_Ticket_System.Exceptions.PassengerExceptions
{
    public class PassengerNotFoundException : Exception
    {
        public PassengerNotFoundException(string passengerId)
            : base($"Passenger with ID {passengerId} was not found.")
        {
        }
    }
}
