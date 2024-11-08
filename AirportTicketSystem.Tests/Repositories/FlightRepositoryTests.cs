using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Models;
using Airport_Ticket_System.RepositoriesHandler;
using Airport_Ticket_System.Exceptions.FlightExceptions;

namespace AirportTicketSystem.Tests.Repositories
{
    public class FlightRepositoryTests : IDisposable
    {
        private readonly string _tempFilePath;
        private readonly FlightRepository _repository;

        public FlightRepositoryTests()
        {
            _tempFilePath = Path.Combine(Path.GetTempPath(), $"test_flights_{Guid.NewGuid()}.csv");
            File.WriteAllText(_tempFilePath, "FlightId;DepartureCountry;DestinationCountry;DepartureDate;DepartureAirport;ArrivalAirport;Price\n");

            FilePathHelper.SetTestFilePath(_tempFilePath);
            _repository = new FlightRepository("test_flights.csv");
        }

        [Fact]
        public void GetAllFlights_ShouldReturnEmptyListWhenNoFlights()
        {

            var flights = _repository.GetAllFlights();


            Assert.Empty(flights);
        }

        [Fact]
        public void AddFlight_ShouldIncreaseFlightCountByOne()
        {

            var initialCount = _repository.GetAllFlights().Count();
            var newFlight = new Flight
            {
                FlightId = "FL001",
                DepartureCountry = "USA",
                DestinationCountry = "UK",
                DepartureDate = DateTime.Today.AddDays(10),
                DepartureAirport = "JFK",
                ArrivalAirport = "LHR",
                Price = 750.00m
            };


            _repository.AddFlight(newFlight);
            var updatedCount = _repository.GetAllFlights().Count();


            Assert.Equal(initialCount + 1, updatedCount);
        }

        [Fact]
        public void AddFlights_ShouldAddMultipleFlights()
        {

            var initialCount = _repository.GetAllFlights().Count();
            var newFlights = new List<Flight>
            {
                new Flight
                {
                    FlightId = "FL002",
                    DepartureCountry = "Canada",
                    DestinationCountry = "Germany",
                    DepartureDate = DateTime.Today.AddDays(15),
                    DepartureAirport = "YYZ",
                    ArrivalAirport = "FRA",
                    Price = 680.00m
                },
                new Flight
                {
                    FlightId = "FL003",
                    DepartureCountry = "Australia",
                    DestinationCountry = "Japan",
                    DepartureDate = DateTime.Today.AddDays(20),
                    DepartureAirport = "SYD",
                    ArrivalAirport = "NRT",
                    Price = 820.00m
                }
            };


            _repository.AddFlights(newFlights);
            var updatedCount = _repository.GetAllFlights().Count();

            Assert.Equal(initialCount + newFlights.Count, updatedCount);
        }

        [Fact]
        public void GetFlightById_ShouldReturnCorrectFlight()
        {

            var flight = new Flight
            {
                FlightId = "FL004",
                DepartureCountry = "France",
                DestinationCountry = "Italy",
                DepartureDate = DateTime.Today.AddDays(5),
                DepartureAirport = "CDG",
                ArrivalAirport = "FCO",
                Price = 300.00m
            };
            _repository.AddFlight(flight);


            var result = _repository.GetFlightById("FL004");


            Assert.Equal("FL004", result.FlightId);
            Assert.Equal("France", result.DepartureCountry);
            Assert.Equal("Italy", result.DestinationCountry);
            Assert.Equal("CDG", result.DepartureAirport);
            Assert.Equal("FCO", result.ArrivalAirport);
            Assert.Equal(300.00m, result.Price);
        }

        [Fact]
        public void GetFlightById_ShouldThrowExceptionIfNotFound()
        {

            var exception = Assert.Throws<FlightNotFoundException>(() => _repository.GetFlightById("NonExistentID"));
            Assert.Equal("Flight with ID NonExistentID was not found.", exception.Message);
        }

        [Fact]
        public void RemoveFlight_ShouldDeleteFlightFromRepository()
        {

            var flight = new Flight
            {
                FlightId = "FL005",
                DepartureCountry = "Spain",
                DestinationCountry = "Portugal",
                DepartureDate = DateTime.Today.AddDays(7),
                DepartureAirport = "MAD",
                ArrivalAirport = "LIS",
                Price = 120.00m
            };
            _repository.AddFlight(flight);
            var initialCount = _repository.GetAllFlights().Count();


            _repository.RemoveFlight("FL005");
            var updatedCount = _repository.GetAllFlights().Count();


            Assert.Equal(initialCount - 1, updatedCount);
            Assert.Throws<FlightNotFoundException>(() => _repository.GetFlightById("FL005"));
        }

        [Fact]
        public void UpdateFlight_ShouldModifyExistingFlight()
        {
            var flight = new Flight
            {
                FlightId = "FL006",
                DepartureCountry = "Brazil",
                DestinationCountry = "Argentina",
                DepartureDate = DateTime.Today.AddDays(12),
                DepartureAirport = "GRU",
                ArrivalAirport = "EZE",
                Price = 250.00m
            };
            _repository.AddFlight(flight);

            flight.Price = 275.00m;
            flight.DestinationCountry = "Chile";


            _repository.UpdateFlight(flight);
            var updatedFlight = _repository.GetFlightById("FL006");


            Assert.Equal("FL006", updatedFlight.FlightId);
            Assert.Equal("Brazil", updatedFlight.DepartureCountry);
            Assert.Equal("Chile", updatedFlight.DestinationCountry);
            Assert.Equal("GRU", updatedFlight.DepartureAirport);
            Assert.Equal("EZE", updatedFlight.ArrivalAirport);
            Assert.Equal(275.00m, updatedFlight.Price);
        }

        [Fact]
        public void GetFlightById_ShouldNotAffectOtherTests()
        {

            var flight = new Flight
            {
                FlightId = "FL007",
                DepartureCountry = "India",
                DestinationCountry = "Singapore",
                DepartureDate = DateTime.Today.AddDays(8),
                DepartureAirport = "DEL",
                ArrivalAirport = "SIN",
                Price = 400.00m
            };
            _repository.AddFlight(flight);


            var allFlights = _repository.GetAllFlights();


            Assert.Contains(allFlights, f => f.FlightId == "FL007");
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
