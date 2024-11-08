using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Interfaces.IServices;
using Airport_Ticket_System.Models;
using Airport_Ticket_System.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace AirportTicketSystem.Tests.Services
{
    public class FlightServiceTests
    {
        private readonly FlightService _flight_service;
        private readonly Mock<IFlightRepository> _flightRepositoryMock;

        public FlightServiceTests()
        {
            _flightRepositoryMock = new Mock<IFlightRepository>();
            _flight_service = new FlightService(_flightRepositoryMock.Object);
        }


/*        [Fact]
        public void ImportFlightsFromCsv_ShouldImportValidFlights()
        {
             
            var filePath = "test_import_flights.csv";
            var csvContent = "FlightId;DepartureCountry;DestinationCountry;DepartureDate;DepartureAirport;ArrivalAirport;Price\n" +
                             "FL001;USA;UK;12/15/2025 8:00:00 AM;JFK;Heathrow;400.00\n" +
                             "FL002;Canada;Germany;12/18/2025 11:00:00 AM;YYZ;Incheon;300.00\n";

            File.WriteAllText(filePath, csvContent);

             
            _flight_service.ImportFlightsFromCsv(filePath);

             
            _flightRepositoryMock.Verify(r => r.AddFlights(It.Is<List<Flight>>(flights =>
                flights.Count == 2 &&
                flights.Exists(f => f.FlightId == "FL001") &&
                flights.Exists(f => f.FlightId == "FL002")
            )), Times.Once);

            File.Delete(filePath); // Clean up
        }*/



        #region SearchFlights Tests

        [Fact]
        public void SearchFlights_ShouldReturnAllFlights_WhenNoFiltersApplied()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL012", DepartureCountry = "USA", DestinationCountry = "UK", DepartureDate = DateTime.Today.AddDays(1), DepartureAirport = "JFK", ArrivalAirport = "LHR", Price = 500.00m },
                new Flight { FlightId = "FL013", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(2), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 800.00m }
            };

            _flightRepositoryMock.Setup(r => r.GetAllFlights()).Returns(flights);

             
            var result = _flight_service.SearchFlights(null!, null!, null, null!, null!, null, null);

             
            Assert.Equal(2, result.Count());
            Assert.Contains(result, f => f.FlightId == "FL012");
            Assert.Contains(result, f => f.FlightId == "FL013");
        }

        [Fact]
        public void SearchFlights_ShouldFilterByDepartureCountry()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL014", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(3), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 700.00m },
                new Flight { FlightId = "FL015", DepartureCountry = "USA", DestinationCountry = "France", DepartureDate = DateTime.Today.AddDays(4), DepartureAirport = "JFK", ArrivalAirport = "CDG", Price = 900.00m },
                new Flight { FlightId = "FL016", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(5), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 1000.00m }
            };

            _flightRepositoryMock.Setup(r => r.GetAllFlights()).Returns(flights);

             
            var result = _flight_service.SearchFlights("USA", null!, null, null!, null!, null, null);

             
            Assert.Equal(2, result.Count());
            Assert.All(result, f => Assert.Equal("USA", f.DepartureCountry, ignoreCase: true));
        }

        [Fact]
        public void SearchFlights_ShouldFilterByDestinationCountry()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL017", DepartureCountry = "USA", DestinationCountry = "Spain", DepartureDate = DateTime.Today.AddDays(6), DepartureAirport = "JFK", ArrivalAirport = "MAD", Price = 800.00m },
                new Flight { FlightId = "FL018", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(7), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 1000.00m }
            };

            _flightRepositoryMock.Setup(r => r.GetAllFlights()).Returns(flights);

             
            var result = _flight_service.SearchFlights(null!, "Germany", null, null!, null!, null, null);

             
            Assert.Single(result);
            Assert.Contains(result, f => f.DestinationCountry.Equals("Germany", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void SearchFlights_ShouldFilterByDepartureDate()
        {
             
            var specificDate = DateTime.Today.AddDays(8).Date;
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL019", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = specificDate, DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 800.00m },
                new Flight { FlightId = "FL020", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(9).Date, DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 1000.00m }
            };

            _flightRepositoryMock.Setup(r => r.GetAllFlights()).Returns(flights);

             
            var result = _flight_service.SearchFlights(null!, null!, specificDate, null!, null!, null, null);

             
            Assert.Single(result);
            Assert.Contains(result, f => f.DepartureDate.Date == specificDate);
        }

        [Fact]
        public void SearchFlights_ShouldFilterByDepartureAirport()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL021", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(10), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 800.00m },
                new Flight { FlightId = "FL022", DepartureCountry = "USA", DestinationCountry = "France", DepartureDate = DateTime.Today.AddDays(11), DepartureAirport = "EWR", ArrivalAirport = "CDG", Price = 900.00m }
            };

            _flightRepositoryMock.Setup(r => r.GetAllFlights()).Returns(flights);

             
            var result = _flight_service.SearchFlights(null!, null!, null, "JFK", null!, null, null);

             
            Assert.Single(result);
            Assert.Contains(result, f => f.DepartureAirport.Equals("JFK", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void SearchFlights_ShouldFilterByArrivalAirport()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL023", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(12), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 800.00m },
                new Flight { FlightId = "FL024", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(13), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 1000.00m }
            };

            _flightRepositoryMock.Setup(r => r.GetAllFlights()).Returns(flights);

             
            var result = _flight_service.SearchFlights(null!, null!, null, null!, "FRA", null, null);

             
            Assert.Single(result);
            Assert.Contains(result, f => f.ArrivalAirport.Equals("FRA", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void SearchFlights_ShouldFilterByMaxPrice()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL025", DepartureCountry = "USA", DestinationCountry = "Italy", DepartureDate = DateTime.Today.AddDays(14), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 800.00m },
                new Flight { FlightId = "FL026", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(15), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 1000.00m }
            };

            _flightRepositoryMock.Setup(r => r.GetAllFlights()).Returns(flights);

             
            var result = _flight_service.SearchFlights(null!, null!, null, null!, null!, null, 900.00m);

             
            Assert.Single(result);
            Assert.Contains(result, f => f.Price <= 900.00m);
        }

        [Fact]
        public void SearchFlights_ShouldBeCaseInsensitive()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL027", DepartureCountry = "usa", DestinationCountry = "italy", DepartureDate = DateTime.Today.AddDays(16), DepartureAirport = "jfk", ArrivalAirport = "fco", Price = 800.00m   },
                new Flight { FlightId = "FL028", DepartureCountry = "USA", DestinationCountry = "ITALY", DepartureDate = DateTime.Today.AddDays(17), DepartureAirport = "JFK", ArrivalAirport = "FCO", Price = 850.00m }
            };

            _flightRepositoryMock.Setup(r => r.GetAllFlights()).Returns(flights);

             
            var result = _flight_service.SearchFlights("UsA", "ItAlY", null, "JFK", "FCO", null, null);

             
            Assert.Equal(2, result.Count());
            Assert.All(result, f => Assert.Equal("USA", f.DepartureCountry, ignoreCase: true));
            Assert.All(result, f => Assert.Equal("ITALY", f.DestinationCountry, ignoreCase: true));
        }

        [Fact]
        public void SearchFlights_ShouldReturnEmpty_WhenNoFlightsMatchFilters()
        {
             
            var flights = new List<Flight>
            {
                new Flight { FlightId = "FL029", DepartureCountry = "USA", DestinationCountry = "UK", DepartureDate = DateTime.Today.AddDays(18), DepartureAirport = "JFK", ArrivalAirport = "LHR", Price = 500.00m },
                new Flight { FlightId = "FL030", DepartureCountry = "Canada", DestinationCountry = "Germany", DepartureDate = DateTime.Today.AddDays(19), DepartureAirport = "YYZ", ArrivalAirport = "FRA", Price = 800.00m }
            };

            _flightRepositoryMock.Setup(r => r.GetAllFlights()).Returns(flights);

             
            var result = _flight_service.SearchFlights(
                departureCountry: "Japan",
                destinationCountry: "France",
                departureDate: DateTime.Today.AddDays(20),
                departureAirport: "NRT",
                arrivalAirport: "CDG",
                flightClass: FlightClass.FirstClass,
                maxPrice: 300.00m
            );

             
            Assert.Empty(result);
        }

        #endregion

        #region PrintValidationDetails Tests

        [Fact]
        public void PrintValidationDetails_ShouldPrintValidationInformation()
        {
             
            var validationDetails = ValidationHelper.GetValidationDetails<Flight>();

            using var sw = new StringWriter();
            Console.SetOut(sw);

             
            _flight_service.PrintValidationDetails();

            // Reset Console Output
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

             
            var output = sw.ToString();
            Assert.Contains("Dynamic Model Validation Details:", output);
            foreach (var detail in validationDetails)
            {
                Assert.Contains($"{detail.Key}:", output);
                Assert.Contains($"Type: {detail.Value.Type}", output);
                Assert.Contains($"Constraint: {detail.Value.Constraints}", output);
            }
        }

        #endregion

        #region Edge Case Tests
        [Fact]
        public void ImportFlightsFromCsv_ShouldThrowException_WhenFileDoesNotExist()
        {
             
            var filePath = "non_existent_file.csv";

             
            var exception = Record.Exception(() => _flight_service.ImportFlightsFromCsv(filePath));

             
            Assert.NotNull(exception);
            Assert.IsType<FileNotFoundException>(exception);
            Assert.Contains("Could not find file", exception.Message);

            _flightRepositoryMock.Verify(r => r.AddFlights(It.IsAny<List<Flight>>()), Times.Never);
        }

        #endregion
    }
}
