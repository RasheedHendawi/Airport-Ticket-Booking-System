using Moq;
using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Interfaces.IServices;
using Airport_Ticket_System.Models;
using Xunit;

namespace AirportTicketSystem.Tests.Helpers
{
    public class PassengerServiceHelperTests
    {
        private readonly Mock<IPassengerService> _passengerServiceMock;
        private readonly Passenger _testPassenger;

        public PassengerServiceHelperTests()
        {
            _passengerServiceMock = new Mock<IPassengerService>();
            _testPassenger = new Passenger { PassengerId = "P001", FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "123-456-7890" };
        }


        #region ManageBookings Tests

/*        [Fact]
        public void ManageBookings_ShouldCancelBookingSuccessfully()
        {
            
            var bookingId = "B003";

            using var consoleInput = new StringReader($"{bookingId}\n"); 
            Console.SetIn(consoleInput);

            
            PassengerServiceHelper.CancelBooking(_passengerServiceMock.Object, "P001");

            
            _passengerServiceMock.Verify(p => p.CancelBooking("P001", bookingId), Times.Once);
        }*/


        [Fact]
        public void ManageBookings_ShouldInformUser_WhenBookingDoesNotExist()
        {
            
            var bookingId = "B999"; 

            _passengerServiceMock.Setup(p => p.CancelBooking("P001", bookingId))
                .Throws(new ArgumentException("Booking not found or you don't have permission to cancel this booking."));

            using var consoleInput = new StringReader($"{bookingId}\n");  
            Console.SetIn(consoleInput);
            using var sw = new StringWriter();
            Console.SetOut(sw);

            
            PassengerServiceHelper.CancelBooking(_passengerServiceMock.Object, "P001");

            
            var output = sw.ToString();
            Assert.Contains("Error: Booking not found or you don't have permission to cancel this booking.", output);
        }

        #endregion
    }
}
