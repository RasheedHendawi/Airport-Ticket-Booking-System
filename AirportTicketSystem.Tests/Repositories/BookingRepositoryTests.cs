using Airport_Ticket_System.Models;
using Airport_Ticket_System.RepositoriesHandler;
using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Exceptions.BookingExceptions;

namespace AirportTicketSystem.Tests.Repositories
{
    public class BookingRepositoryTests : IDisposable
    {
        private readonly string _tempFilePath;
        private readonly BookingRepository _repository;

        public BookingRepositoryTests()
        {
            _tempFilePath = Path.Combine(Path.GetTempPath(), $"test_bookings_{Guid.NewGuid()}.csv");

            File.WriteAllText(_tempFilePath, "BookingId;PassengerId;FlightId;BookingDate;FlightClass;Price\n");

            FilePathHelper.SetTestFilePath(_tempFilePath);
            _repository = new BookingRepository("test_bookings.csv");
        }

        [Fact]
        public void GetAllBookings_ShouldReturnEmptyListWhenNoBookings()
        {

            var bookings = _repository.GetAllBookings();


            Assert.Empty(bookings);
        }

        [Fact]
        public void AddBooking_ShouldIncreaseBookingCountByOne()
        {

            var initialCount = _repository.GetAllBookings().Count();
            var newBooking = new Booking
            {
                BookingId = "B001",
                PassengerId = "P001",
                FlightId = "FL001",
                BookingDate = DateTime.Today,
                Class = FlightClass.Economy,
                Price = 200.00m
            };


            _repository.AddBooking(newBooking);
            var updatedCount = _repository.GetAllBookings().Count();


            Assert.Equal(initialCount + 1, updatedCount);
        }

        [Fact]
        public void GetBookingById_ShouldReturnCorrectBooking()
        {

            var booking = new Booking
            {
                BookingId = "B002",
                PassengerId = "P002",
                FlightId = "FL002",
                BookingDate = DateTime.Today.AddDays(1),
                Class = FlightClass.Business,
                Price = 500.00m
            };
            _repository.AddBooking(booking);


            var result = _repository.GetBookingById("B002");

            Assert.Equal("B002", result.BookingId);
            Assert.Equal("P002", result.PassengerId);
            Assert.Equal("FL002", result.FlightId);
            Assert.Equal(FlightClass.Business, result.Class);
            Assert.Equal(500.00m, result.Price);
        }

        [Fact]
        public void GetBookingById_ShouldThrowExceptionIfNotFound()
        {
            var exception = Assert.Throws<BookingNotFoundException>(() => _repository.GetBookingById("NonExistentID"));
            Assert.Equal("Booking with ID NonExistentID not found.", exception.Message);
        }

        [Fact]
        public void RemoveBooking_ShouldDeleteBookingFromRepository()
        {
            var booking = new Booking
            {
                BookingId = "B003",
                PassengerId = "P003",
                FlightId = "FL003",
                BookingDate = DateTime.Today.AddDays(2),
                Class = FlightClass.FirstClass,
                Price = 800.00m
            };
            _repository.AddBooking(booking);
            var initialCount = _repository.GetAllBookings().Count();

            _repository.RemoveBooking("B003");
            var updatedCount = _repository.GetAllBookings().Count();


            Assert.Equal(initialCount - 1, updatedCount);
            Assert.Throws<BookingNotFoundException>(() => _repository.GetBookingById("B003"));
        }

        [Fact]
        public void UpdateBooking_ShouldModifyExistingBooking()
        {
            var booking = new Booking
            {
                BookingId = "B004",
                PassengerId = "P004",
                FlightId = "FL004",
                BookingDate = DateTime.Today.AddDays(3),
                Class = FlightClass.Economy,
                Price = 150.00m
            };
            _repository.AddBooking(booking);

            booking.Class = FlightClass.Business;
            booking.Price = 300.00m;


            _repository.UpdateBooking(booking);
            var updatedBooking = _repository.GetBookingById("B004");


            Assert.Equal(FlightClass.Business, updatedBooking.Class);
            Assert.Equal(300.00m, updatedBooking.Price);
        }


        [Fact]
        public void RemoveBooking_ShouldThrowExceptionIfBookingNotFound()
        {
            var exception = Assert.Throws<BookingNotFoundException>(() => _repository.RemoveBooking("NonExistentID"));
            Assert.Equal("Booking with ID NonExistentID not found.", exception.Message);
        }

        public void Dispose()
        {

            if (File.Exists(_tempFilePath))
            {
                try
                {
                    File.Delete(_tempFilePath);
                }
                catch (IOException)
                {
                    System.Threading.Thread.Sleep(50);
                    File.Delete(_tempFilePath);
                }
            }

            FilePathHelper.SetTestFilePath(null!);
        }
    }
}
