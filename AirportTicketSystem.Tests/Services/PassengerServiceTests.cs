using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using Airport_Ticket_System.Exceptions.FlightExceptions;
using Airport_Ticket_System.Exceptions.BookingExceptions;
using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Models;
using Airport_Ticket_System.Services;

namespace AirportTicketSystem.Tests.Services
{
    public class PassengerServiceTests
    {
        private readonly Mock<IFlightRepository> _mockFlightRepo;
        private readonly Mock<IBookingRepository> _mockBookingRepo;
        private readonly PassengerService _passengerService;

        public PassengerServiceTests()
        {
            _mockFlightRepo = new Mock<IFlightRepository>();
            _mockBookingRepo = new Mock<IBookingRepository>();
            _passengerService = new PassengerService(_mockFlightRepo.Object, _mockBookingRepo.Object);
        }

        #region SearchAvailableFlights Tests

        [Fact]
        public void SearchAvailableFlights_ShouldReturnAllFlights_WhenNoFiltersApplied()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL001", DepartureCountry = "USA", DestinationCountry = "UK", DepartureDate = DateTime.Today.AddDays(1), DepartureAirport = "JFK", ArrivalAirport = "LHR", Price = 500.00m },
                new Flight { FlightId = "FL002", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(2), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 800.00m }
            };

            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

             
            var result = _passengerService.SearchAvailableFlights();

             
            Assert.Equal(2, result.Count());
            Assert.Contains(result, f => f.FlightId == "FL001");
            Assert.Contains(result, f => f.FlightId == "FL002");
        }

        [Fact]
        public void SearchAvailableFlights_ShouldFilterByMaxPrice()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL003", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(3), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 300.00m },
                new Flight { FlightId = "FL004", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(4), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 600.00m }
            };

            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

             
            var result = _passengerService.SearchAvailableFlights(maxPrice: 400.00m);

             
            Assert.Single(result);
            Assert.Contains(result, f => f.FlightId == "FL003");
        }

        [Fact]
        public void SearchAvailableFlights_ShouldFilterByDepartureCountry()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL005", DepartureCountry = "USA", DestinationCountry = "Spain", DepartureDate = DateTime.Today.AddDays(5), DepartureAirport = "JFK", ArrivalAirport = "MAD", Price = 500.00m },
                new Flight { FlightId = "FL006", DepartureCountry = "USA", DestinationCountry = "France", DepartureDate = DateTime.Today.AddDays(6), DepartureAirport = "JFK", ArrivalAirport = "CDG", Price = 700.00m },
                new Flight { FlightId = "FL007", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(7), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 900.00m }
            };

            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

             
            var result = _passengerService.SearchAvailableFlights(departureCountry: "USA");

             
            Assert.Equal(2, result.Count());
            Assert.All(result, f => Assert.Equal("USA", f.DepartureCountry));
        }

        [Fact]
        public void SearchAvailableFlights_ShouldFilterByDestinationCountry()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL008", DepartureCountry = "USA", DestinationCountry = "Spain", DepartureDate = DateTime.Today.AddDays(8), DepartureAirport = "JFK", ArrivalAirport = "MAD", Price = 500.00m },
                new Flight { FlightId = "FL009", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(9), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 900.00m }
            };

            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

             
            var result = _passengerService.SearchAvailableFlights(destinationCountry: "Germany");

             
            Assert.Single(result);
            Assert.Contains(result, f => f.DestinationCountry == "Germany");
        }

        [Fact]
        public void SearchAvailableFlights_ShouldFilterByDepartureDate()
        {
             
            var specificDate = DateTime.Today.AddDays(10);
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL010", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = specificDate, DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 800.00m },
                new Flight { FlightId = "FL011", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(11), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 1000.00m }
            };

            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

             
            var result = _passengerService.SearchAvailableFlights(departureDate: specificDate);

             
            Assert.Single(result);
            Assert.Contains(result, f => f.DepartureDate.Date == specificDate.Date);
        }

        [Fact]
        public void SearchAvailableFlights_ShouldFilterByDepartureAirport()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL012", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(12), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 800.00m },
                new Flight { FlightId = "FL013", DepartureCountry = "USA", DestinationCountry = "France", DepartureDate = DateTime.Today.AddDays(13), DepartureAirport = "EWR", ArrivalAirport = "CDG", Price = 900.00m }
            };

            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

             
            var result = _passengerService.SearchAvailableFlights(departureAirport: "JFK");

             
            Assert.Single(result);
            Assert.Contains(result, f => f.DepartureAirport == "JFK");
        }

        [Fact]
        public void SearchAvailableFlights_ShouldFilterByArrivalAirport()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL014", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(14), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 800.00m },
                new Flight { FlightId = "FL015", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(15), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 1000.00m }
            };

            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

             
            var result = _passengerService.SearchAvailableFlights(arrivalAirport: "FRA");

             
            Assert.Single(result);
            Assert.Contains(result, f => f.ArrivalAirport == "FRA");
        }


        [Fact]
        public void SearchAvailableFlights_ShouldFilterByMultipleCriteria()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL018", DepartureCountry = "USA", DestinationCountry = "Palestine", DepartureDate = DateTime.Today.AddDays(18), DepartureAirport = "JFK", ArrivalAirport = "MAD", Price = 800.00m },
                new Flight { FlightId = "FL019", DepartureCountry = "USA", DestinationCountry = "Spain", DepartureDate = DateTime.Today.AddDays(18), DepartureAirport = "JFK", ArrivalAirport = "MAD", Price = 800.00m },
                new Flight { FlightId = "FL020", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(19), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 1000.00m }
            };

            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

             
            var result = _passengerService.SearchAvailableFlights(
                maxPrice: 900.00m,
                departureCountry: "USA",
                destinationCountry: "Palestine",
                departureDate: DateTime.Today.AddDays(18),
                departureAirport: "JFK",
                arrivalAirport: "MAD"
            );

             
            Assert.Single(result);
            var flight = result.First();
            Assert.Equal("FL018", flight.FlightId);
        }

        [Fact]
        public void SearchAvailableFlights_ShouldReturnEmpty_WhenNoFlightsMatchFilters()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL021", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(20), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 800.00m},
                new Flight { FlightId = "FL022", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(21), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 1000.00m }
            };

            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

             
            var result = _passengerService.SearchAvailableFlights(
                maxPrice: 700.00m,
                departureCountry: "USA",
                destinationCountry: "France",
                departureDate: DateTime.Today.AddDays(22),
                departureAirport: "EWR",
                arrivalAirport: "CDG",
                flightClass: "FirstClass"
            );

             
            Assert.Empty(result);
        }

        #endregion

        #region CancelBooking Tests


        [Fact]
        public void CancelBooking_ShouldThrowException_WhenBookingDoesNotExist()
        {
             
            var passengerId = "P123";
            var bookingId = "NonExistentBooking";

            _mockBookingRepo.Setup(repo => repo.GetBookingById(bookingId)).Returns((Booking)null!);

            var exception = Assert.Throws<ArgumentException>(() => _passengerService.CancelBooking(passengerId, bookingId));
            Assert.Equal("Booking not found or you don't have permission to cancel this booking.", exception.Message);
        }

        [Fact]
        public void CancelBooking_ShouldThrowException_WhenBookingDoesNotBelongToPassenger()
        {
             
            var passengerId = "P124";
            var bookingId = "B124";
            var booking = new Booking
            {
                BookingId = bookingId,
                PassengerId = "P999", 
                FlightId = "FL002",
                BookingDate = DateTime.Today.AddDays(-1),
                Class = FlightClass.Business,
                Price = 800.00m
            };

            _mockBookingRepo.Setup(repo => repo.GetBookingById(bookingId)).Returns(booking);

            var exception = Assert.Throws<ArgumentException>(() => _passengerService.CancelBooking(passengerId, bookingId));
            Assert.Equal("Booking not found or you don't have permission to cancel this booking.", exception.Message);
        }

        #endregion

        #region ViewBookings Tests

        [Fact]
        public void ViewBookings_ShouldReturnBookings_ForValidPassengerId()
        {
             
            var passengerId = "P125";
            var bookings = new List<Booking>
            {
                new Booking { BookingId = "B125", PassengerId = passengerId, FlightId = "FL003", BookingDate = DateTime.Today, Class = FlightClass.Economy, Price = 500.00m },
                new Booking { BookingId = "B126", PassengerId = passengerId, FlightId = "FL004", BookingDate = DateTime.Today.AddDays(1), Class = FlightClass.Business, Price = 1000.00m }
            };

            _mockBookingRepo.Setup(repo => repo.GetBookingByPassengerId(passengerId)).Returns(bookings);

             
            var result = _passengerService.ViewBookings(passengerId);

             
            Assert.Equal(2, result.Count());
            Assert.All(result, b => Assert.Equal(passengerId, b.PassengerId));
        }

        [Fact]
        public void ViewBookings_ShouldReturnEmpty_WhenNoBookingsExist()
        {
             
            var passengerId = "P126";
            _mockBookingRepo.Setup(repo => repo.GetBookingByPassengerId(passengerId)).Returns(new List<Booking>());

             
            var result = _passengerService.ViewBookings(passengerId);

             
            Assert.Empty(result);
        }

        #endregion

        #region ModifyBooking Tests

        [Fact]
        public void ModifyBooking_ShouldThrowException_WhenBookingDoesNotExist()
        {
             
            var passengerId = "P128";
            var bookingId = "B128";
            var newFlightId = "FL007";
            var newFlightClass = FlightClass.Business;

            _mockBookingRepo.Setup(repo => repo.GetBookingById(bookingId)).Returns((Booking)null!);

            var exception = Assert.Throws<ArgumentException>(() => _passengerService.ModifyBooking(passengerId, bookingId, newFlightId, newFlightClass));
            Assert.Equal("Booking not found or you don't have permission to modify this booking.", exception.Message);
        }

        [Fact]
        public void ModifyBooking_ShouldThrowFlightNotFoundException_WhenNewFlightDoesNotExist()
        {
             
            var passengerId = "P129";
            var bookingId = "B129";
            var newFlightId = "FL999"; 
            var newFlightClass = FlightClass.Business;

            var existingBooking = new Booking
            {
                BookingId = bookingId,
                PassengerId = passengerId,
                FlightId = "FL008",
                BookingDate = DateTime.Today.AddDays(-1),
                Class = FlightClass.Economy,
                Price = 500.00m
            };

            _mockBookingRepo.Setup(repo => repo.GetBookingById(bookingId)).Returns(existingBooking);
            _mockFlightRepo.Setup(repo => repo.GetFlightById(newFlightId)).Returns((Flight)null!);

            var exception = Assert.Throws<FlightNotFoundException>(() => _passengerService.ModifyBooking(passengerId, bookingId, newFlightId, newFlightClass));
            Assert.Equal($"Flight with ID {newFlightId} was not found.", exception.Message);
        }

        #endregion

        #region BookFlight Tests

        [Fact]
        public void BookFlight_ShouldAddBooking_WhenValidInputs()
        {
             
            var passengerId = "P130";
            var flightId = "FL009";
            var flightClass = FlightClass.Economy;

            var flight = new Flight
            {
                FlightId = flightId,
                DepartureCountry = "USA",
                DestinationCountry = "France",
                DepartureDate = DateTime.Today.AddDays(5),
                DepartureAirport = "JFK",
                ArrivalAirport = "CDG",
                Price = 700.00m
            };

            _mockFlightRepo.Setup(repo => repo.GetFlightById(flightId)).Returns(flight);

            _passengerService.BookFlight(passengerId, flightId, flightClass);

            _mockBookingRepo.Verify(repo => repo.AddBooking(It.Is<Booking>(
                b => b.PassengerId == passengerId &&
                     b.FlightId == flightId &&
                     b.Class == flightClass &&
                     b.Price == FlightClassHelper.GetFlightPrice(flight.Price, flightClass)
            )), Times.Once);
        }

        [Fact]
        public void BookFlight_ShouldThrowFlightNotFoundException_WhenFlightDoesNotExist()
        {
            var passengerId = "P131";
            var flightId = "FL999";
            var flightClass = FlightClass.Economy;

            _mockFlightRepo.Setup(repo => repo.GetFlightById(flightId)).Returns((Flight)null!);

            var exception = Assert.Throws<FlightNotFoundException>(() => _passengerService.BookFlight(passengerId, flightId, flightClass));
            Assert.Equal($"Flight with ID {flightId} was not found.", exception.Message);
        }

        #endregion

        #region PrintAllFlights Tests

        [Fact]
        public void PrintAllFlights_ShouldPrintAllFlights_WhenFlightsExist()
        {
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL010", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(20), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 800.00m },
                new Flight { FlightId = "FL011", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(21), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 1000.00m }
            };

            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                _passengerService.PrintAllFlights();

                var output = sw.ToString();
                Assert.Contains("Available Flights:", output);
                Assert.Contains("Flight ID:           FL010", output);
                Assert.Contains("Flight ID:           FL011", output);
                Assert.Contains($"Price:               {flights[0].Price:C}", output);
                Assert.Contains($"Price:               {flights[1].Price:C}", output);
            }
        }

        [Fact]
        public void PrintAllFlights_ShouldInformNoFlights_WhenNoFlightsExist()
        {
            var flights = new List<Flight>();

            _mockFlightRepo.Setup(repo => repo.GetAllFlights()).Returns(flights);

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                _passengerService.PrintAllFlights();

                var output = sw.ToString();
                Assert.Contains("No flights available at the moment.", output);
            }
        }

        #endregion

        #region GeneratePassenger Tests (Static Method)

        [Fact]
        public void GeneratePassenger_ShouldAddPassengerToRepository()
        {
            var firstName = "Alice";
            var lastName = "Johnson";
            var email = "alice.johnson@example.com";
            var phoneNumber = "555-7890";



            var passenger = PassengerService.GenaratePassenger(firstName, lastName, email, phoneNumber);

            Assert.NotNull(passenger);
            Assert.Equal(firstName, passenger.FirstName);
            Assert.Equal(lastName, passenger.LastName);
            Assert.Equal(email, passenger.Email);
            Assert.Equal(phoneNumber, passenger.PhoneNumber);
            Assert.False(string.IsNullOrEmpty(passenger.PassengerId));

        }


        #endregion
    }
}
