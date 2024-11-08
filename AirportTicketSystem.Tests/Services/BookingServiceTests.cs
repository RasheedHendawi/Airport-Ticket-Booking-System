using Moq;
using Airport_Ticket_System.Exceptions.BookingExceptions;
using Airport_Ticket_System.Exceptions.FlightExceptions;
using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Models;
using Airport_Ticket_System.Services;

namespace AirportTicketSystem.Tests.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _mockBookingRepo;
        private readonly Mock<IFlightRepository> _mockFlightRepo;
        private readonly BookingService _bookingService;

        public BookingServiceTests()
        {
            _mockBookingRepo = new Mock<IBookingRepository>();
            _mockFlightRepo = new Mock<IFlightRepository>();
            _bookingService = new BookingService(_mockBookingRepo.Object, _mockFlightRepo.Object);
        }

        #region BookFlight Tests


        [Fact]
        public void BookFlight_ShouldThrowFlightNotFoundException_WhenFlightDoesNotExist()
        {
            _mockFlightRepo.Setup(repo => repo.GetFlightById("InvalidFL")).Returns((Flight)null!);

            var exception = Assert.Throws<FlightNotFoundException>(() => _bookingService.BookFlight("P002", "InvalidFL", FlightClass.Business));
            Assert.Equal("Flight with ID InvalidFL was not found.", exception.Message);
        }

        #endregion

        #region GetBookingsByPassengerId Tests

        [Fact]
        public void GetBookingsByPassengerId_ShouldReturnBookings_WhenBookingsExist()
        {
            var bookings = new List<Booking>
            {
                new Booking { BookingId = "B001", PassengerId = "P001", FlightId = "FL001", BookingDate = DateTime.Today, Class = FlightClass.Economy, Price = 300.00m },
                new Booking { BookingId = "B002", PassengerId = "P001", FlightId = "FL002", BookingDate = DateTime.Today.AddDays(1), Class = FlightClass.Business, Price = 600.00m }
            };

            _mockBookingRepo.Setup(repo => repo.GetBookingByPassengerId("P001")).Returns(bookings);

            var result = _bookingService.GetBookingsByPassengerId("P001");

            Assert.Equal(2, result.Count());
            Assert.Contains(result, b => b.BookingId == "B001");
            Assert.Contains(result, b => b.BookingId == "B002");
        }

        [Fact]
        public void GetBookingsByPassengerId_ShouldReturnEmpty_WhenNoBookingsExist()
        {

            _mockBookingRepo.Setup(repo => repo.GetBookingByPassengerId("P999")).Returns(new List<Booking>());


            var result = _bookingService.GetBookingsByPassengerId("P999");


            Assert.Empty(result);
        }

        #endregion


        #region FilterBookings Tests

        [Fact]
        public void FilterBookings_ShouldFilterByFlightId()
        {
            var bookings = new List<Booking>
            {
                new Booking { BookingId = "B008", PassengerId = "P008", FlightId = "FL010", BookingDate = DateTime.Today, Class = FlightClass.Economy, Price = 300.00m },
                new Booking { BookingId = "B009", PassengerId = "P009", FlightId = "FL011", BookingDate = DateTime.Today.AddDays(1), Class = FlightClass.Business, Price = 600.00m }
            };

            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL010", DepartureCountry = "USA", DestinationCountry = "Spain", DepartureDate = DateTime.Today.AddDays(5), DepartureAirport = "JFK", ArrivalAirport = "MAD", Price = 300.00m },
                new Flight { FlightId = "FL011", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(10), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 600.00m }
            };

            _mockBookingRepo.Setup(repo => repo.GetAllBookings()).Returns(bookings);
            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

            var result = _bookingService.FilterBookings(flightId: "FL010");

            Assert.Single(result);
            Assert.Contains(result, b => b.FlightId == "FL010");
        }

        [Fact]
        public void FilterBookings_ShouldFilterByMultipleCriteria()
        {
            var bookings = new List<Booking>
            {
                new Booking { BookingId = "B010", PassengerId = "P010", FlightId = "FL012", BookingDate = DateTime.Today, Class = FlightClass.Economy, Price = 300.00m },
                new Booking { BookingId = "B011", PassengerId = "P010", FlightId = "FL013", BookingDate = DateTime.Today.AddDays(1), Class = FlightClass.Business, Price = 600.00m },
                new Booking { BookingId = "B012", PassengerId = "P012", FlightId = "FL012", BookingDate = DateTime.Today.AddDays(2), Class = FlightClass.FirstClass, Price = 900.00m }
            };

            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL012", DepartureCountry = "USA", DestinationCountry = "Spain", DepartureDate = DateTime.Today.AddDays(5), DepartureAirport = "JFK", ArrivalAirport = "MAD", Price = 300.00m },
                new Flight { FlightId = "FL013", DepartureCountry = "USA", DestinationCountry = "Spain", DepartureDate = DateTime.Today.AddDays(5), DepartureAirport = "JFK", ArrivalAirport = "MAD", Price = 600.00m }
            };

            _mockBookingRepo.Setup(repo => repo.GetAllBookings()).Returns(bookings);
            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

            var result = _bookingService.FilterBookings(
                passengerId: "P010",
                maxPrice: 600.00m,
                departureCountry: "USA",
                destinationCountry: "Spain",
                departureDate: DateTime.Today.AddDays(5),
                departureAirport: "JFK",
                arrivalAirport: "MAD",
                flightClass: "Business"
            );

            Assert.Single(result);
            var booking = result.First();
            Assert.Equal("B011", booking.BookingId);
            Assert.Equal("P010", booking.PassengerId);
            Assert.Equal("FL013", booking.FlightId);
            Assert.Equal(FlightClass.Business, booking.Class);
            Assert.Equal(600.00m, booking.Price);
        }

        #endregion

        #region GetBookingsByPassengerId Tests

        [Fact]
        public void GetBookingsByPassengerId_ShouldReturnCorrectBookings()
        {
            var bookings = new List<Booking>
            {
                new Booking { BookingId = "B015", PassengerId = "P015", FlightId = "FL016", BookingDate = DateTime.Today, Class = FlightClass.Economy, Price = 300.00m },
                new Booking { BookingId = "B016", PassengerId = "P015", FlightId = "FL017", BookingDate = DateTime.Today.AddDays(1), Class = FlightClass.Business, Price = 600.00m }
            };

            _mockBookingRepo.Setup(repo => repo.GetBookingByPassengerId("P015")).Returns(bookings);

            var result = _bookingService.GetBookingsByPassengerId("P015");

            Assert.Equal(2, result.Count());
            Assert.All(result, b => Assert.Equal("P015", b.PassengerId));
        }

        [Fact]
        public void GetBookingsByPassengerId_ShouldReturnEmpty_WhenNoBookings()
        {
            _mockBookingRepo.Setup(repo => repo.GetBookingByPassengerId("P999")).Returns(new List<Booking>());

            var result = _bookingService.GetBookingsByPassengerId("P999");

            Assert.Empty(result);
        }

        #endregion

        #region CancelBooking Tests

        [Fact]
        public void CancelBooking_ShouldRemoveBooking_WhenBookingExists()
        {
            var booking = new Booking
            {
                BookingId = "B017",
                PassengerId = "P017",
                FlightId = "FL018",
                BookingDate = DateTime.Today.AddDays(-3),
                Class = FlightClass.Economy,
                Price = 300.00m
            };

            _mockBookingRepo.Setup(repo => repo.GetBookingById("B017")).Returns(booking);

            _bookingService.CancelBooking("B017");

            _mockBookingRepo.Verify(repo => repo.RemoveBooking("B017"), Times.Once);
        }

        [Fact]
        public void CancelBooking_ShouldThrowBookingNotFoundException_WhenBookingDoesNotExist()
        {
            _mockBookingRepo.Setup(repo => repo.GetBookingById("B999")).Returns((Booking)null!);

            var exception = Assert.Throws<BookingNotFoundException>(() => _bookingService.CancelBooking("B999"));
            Assert.Equal("Booking with ID B999 not found.", exception.Message);
        }

        #endregion

        #region ModifyBooking Tests

        [Fact]
        public void ModifyBooking_ShouldThrowBookingNotFoundException_WhenBookingDoesNotExist()
        {
            _mockBookingRepo.Setup(repo => repo.GetBookingById("B100")).Returns((Booking)null!);

            var exception = Assert.Throws<BookingNotFoundException>(() => _bookingService.ModifyBooking("B100", "FL019", FlightClass.FirstClass));
            Assert.Equal("Booking with ID B100 not found.", exception.Message);
        }

        [Fact]
        public void ModifyBooking_ShouldThrowFlightNotFoundException_WhenNewFlightDoesNotExist()
        {
            var existingBooking = new Booking
            {
                BookingId = "B101",
                PassengerId = "P101",
                FlightId = "FL020",
                BookingDate = DateTime.Today.AddDays(-1),
                Class = FlightClass.Economy,
                Price = 300.00m
            };

            _mockBookingRepo.Setup(repo => repo.GetBookingById("B101")).Returns(existingBooking);
            _mockFlightRepo.Setup(repo => repo.GetFlightById("FL999")).Returns((Flight)null!);

            var exception = Assert.Throws<FlightNotFoundException>(() => _bookingService.ModifyBooking("B101", "FL999", FlightClass.Business));
            Assert.Equal("Flight with ID FL999 was not found.", exception.Message);
        }

        #endregion

        #region FilterBookings Tests

        [Fact]
        public void FilterBookings_ShouldReturnAllBookings_WhenNoFiltersApplied()
        {
            var bookings = new List<Booking>
            {
                new Booking { BookingId = "B102", PassengerId = "P102", FlightId = "FL021", BookingDate = DateTime.Today, Class = FlightClass.Economy, Price = 300.00m },
                new Booking { BookingId = "B103", PassengerId = "P103", FlightId = "FL022", BookingDate = DateTime.Today.AddDays(1), Class = FlightClass.Business, Price = 600.00m }
            };

            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL021", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(7), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 300.00m },
                new Flight { FlightId = "FL022", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(10), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 600.00m }
            };

            _mockBookingRepo.Setup(repo => repo.GetAllBookings()).Returns(bookings);
            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

            var result = _bookingService.FilterBookings();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void FilterBookings_ShouldFilterByPassengerId()
        {
            var bookings = new List<Booking>
            {
                new Booking { BookingId = "B104", PassengerId = "P104", FlightId = "FL023", BookingDate = DateTime.Today, Class = FlightClass.Economy, Price = 300.00m },
                new Booking { BookingId = "B105", PassengerId = "P105", FlightId = "FL024", BookingDate = DateTime.Today.AddDays(1), Class = FlightClass.Business, Price = 600.00m }
            };

            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL023", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(7), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 300.00m },
                new Flight { FlightId = "FL024", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(10), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 600.00m }
            };

            _mockBookingRepo.Setup(repo => repo.GetAllBookings()).Returns(bookings);
            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

            var result = _bookingService.FilterBookings(passengerId: "P104");

            Assert.Single(result);
            Assert.Equal("B104", result.First().BookingId);
        }

        [Fact]
        public void FilterBookings_ShouldFilterByFlightClass()
        {
            var bookings = new List<Booking>
            {
                new Booking { BookingId = "B106", PassengerId = "P106", FlightId = "FL025", BookingDate = DateTime.Today, Class = FlightClass.Economy, Price = 300.00m },
                new Booking { BookingId = "B107", PassengerId = "P107", FlightId = "FL026", BookingDate = DateTime.Today.AddDays(1), Class = FlightClass.Business, Price = 600.00m }
            };

            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL025", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(7), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 300.00m },
                new Flight { FlightId = "FL026", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(10), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 600.00m }
            };

            _mockBookingRepo.Setup(repo => repo.GetAllBookings()).Returns(bookings);
            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

            var result = _bookingService.FilterBookings(flightClass: "Business");

            Assert.Single(result);
            Assert.Equal(FlightClass.Business, result.First().Class);
        }

        [Fact]
        public void FilterBookings_ShouldReturnEmpty_WhenNoBookingsMatchFilters()
        {
            var bookings = new List<Booking>
            {
                new Booking { BookingId = "B108", PassengerId = "P108", FlightId = "FL027", BookingDate = DateTime.Today, Class = FlightClass.Economy, Price = 300.00m }
            };

            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL027", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(7), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 300.00m }
            };

            _mockBookingRepo.Setup(repo => repo.GetAllBookings()).Returns(bookings);
            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

            var result = _bookingService.FilterBookings(flightId: "FL999");

            Assert.Empty(result);
        }

        

        #endregion

        #region PrintBookings Tests



        #endregion
    }
}
