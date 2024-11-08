using Airport_Ticket_System.Exceptions.FlightExceptions;
using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.RepositoriesHandler
{
    public class FlightRepository : IFlightRepository
    {
        private readonly List<Flight> _flights;
        private readonly string _filePath;

        public FlightRepository(string filePath)
        {
            _filePath = FilePathHelper.GetDataFilePath(filePath);
            _flights = LoadFlightsFromFile();
        }


        private List<Flight> LoadFlightsFromFile()
        {
            if (!File.Exists(_filePath))
                return [];

            return File.ReadAllLines(_filePath)
                       .Skip(1)
                       .Select(line => CsvToFlight(line))
                       .ToList();
        }


        private static Flight CsvToFlight(string csvLine)
        {
            var values = csvLine.Split(';');
            return new Flight
            {
                FlightId = values[0],
                DepartureCountry = values[1],
                DestinationCountry = values[2],
                DepartureDate = DateTime.Parse(values[3]),
                DepartureAirport = values[4],
                ArrivalAirport = values[5],
                Price = decimal.Parse(values[6])
            };
        }

        private void SaveFlightsToFile()
        {
            var lines = new List<string>
            {
                "FlightId;DepartureCountry;DestinationCountry;DepartureDate;DepartureAirport;ArrivalAirport;Price"
            };

            lines.AddRange(_flights.Select(f => $"{f.FlightId};{f.DepartureCountry};{f.DestinationCountry};{f.DepartureDate};{f.DepartureAirport};{f.ArrivalAirport};{f.Price}"));
            File.WriteAllLines(_filePath, lines);
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            return _flights;
        }

        public Flight GetFlightById(string flightId)
        {
            var flight = _flights.FirstOrDefault(f => f.FlightId == flightId);
            return flight ?? throw new FlightNotFoundException(flightId);
        }

        public void AddFlight(Flight flight)
        {
            _flights.Add(flight);
            SaveFlightsToFile();
        }
        public void AddFlights(List<Flight> flight)
        {
            _flights.AddRange(flight);
            SaveFlightsToFile();
        }

        public void RemoveFlight(string flightId)
        {
            var flight = GetFlightById(flightId);
            _flights.Remove(flight);
            SaveFlightsToFile();
        }

        public void UpdateFlight(Flight flight)
        {
            var existingFlight = GetFlightById(flight.FlightId);
            _flights[_flights.IndexOf(existingFlight)] = flight;
            SaveFlightsToFile();
        }

    }

}
