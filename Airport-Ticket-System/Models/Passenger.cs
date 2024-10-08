namespace Airport_Ticket_System.Models
{
    public class Passenger
    {
        public required string PassengerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }             
        public required string PhoneNumber { get; set; }
    }
}
