using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Interfaces.IServices;
using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.Services
{
    public class FlightService(IFlightRepository flightRepository) : IFlightService
    {
        private readonly IFlightRepository _flightRepository = flightRepository;

        public void ImportFlightsFromCsv(string filePath)
        {
            var flights = new List<Flight>();
            var errors = new Dictionary<int, List<string>>();
            int rowIndex = 1;
            string atFilePath = $@"{filePath}";
            using (var reader = new StreamReader(atFilePath))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line!.Split(';');

                    var flight = new Flight
                    {
                        FlightId = values[0],
                        DepartureCountry = values[1],
                        DestinationCountry = values[2],
                        DepartureDate = DateTime.Parse(values[3]),
                        DepartureAirport = values[4],
                        ArrivalAirport = values[5],
                        Price = decimal.Parse(values[6])
                    };

                    var validationErrors = ValidateFlight(flight);
                    if (validationErrors.Count != 0)
                    {
                        errors.Add(rowIndex, validationErrors);
                        foreach (var err in validationErrors)
                        {
                            Logger.LogError($"Row {rowIndex}: {err}");
                        }
                    }
                    else
                    {
                        flights.Add(flight);
                    }

                    rowIndex++;
                }
            }

            if (errors.Count != 0)
            {
                Console.WriteLine("Flight Import Errors:");
                foreach (var error in errors)
                {
                    Console.WriteLine($"Row {error.Key}:");
                    foreach (var err in error.Value)
                    {
                        Console.WriteLine($" - {err}");
                    }
                }
            }
            else
            {
                _flightRepository.AddFlights(flights);
                Console.WriteLine("Flights imported successfully.");
            }
        }


        private static List<string> ValidateFlight(Flight flight)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(flight.DepartureCountry))
                errors.Add("Departure Country is required.");

            if (string.IsNullOrWhiteSpace(flight.DestinationCountry))
                errors.Add("Destination Country is required.");

            if (flight.DepartureDate == default)
                errors.Add("Departure Date is required.");

            else if (flight.DepartureDate < DateTime.Today)
                errors.Add("Departure Date must be today or in the future.");

            if (string.IsNullOrWhiteSpace(flight.DepartureAirport))
                errors.Add("Departure Airport is required.");

            if (string.IsNullOrWhiteSpace(flight.ArrivalAirport))
                errors.Add("Arrival Airport is required.");

            if (flight.Price <= 0)
                errors.Add("Price must be greater than 0.");


            return errors;
        }


        public IEnumerable<Flight> SearchFlights(
            string departureCountry,
            string destinationCountry,
            DateTime? departureDate,
            string departureAirport,
            string arrivalAirport,
            FlightClass? flightClass,
            decimal? maxPrice)
        {
            var flights = _flightRepository.GetAllFlights();

            if (!string.IsNullOrEmpty(departureCountry))
                flights = flights.Where(f => f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(destinationCountry))
                flights = flights.Where(f => f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase));

            if (departureDate.HasValue)
                flights = flights.Where(f => f.DepartureDate.Date == departureDate.Value.Date);

            if (!string.IsNullOrEmpty(departureAirport))
                flights = flights.Where(f => f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(arrivalAirport))
                flights = flights.Where(f => f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase));

            if (maxPrice.HasValue)
                flights = flights.Where(f => f.Price <= maxPrice.Value);

            return flights.ToList();
        }

        public void PrintValidationDetails()
        {
            var validationDetails = ValidationHelper.GetValidationDetails<Flight>();

            Console.WriteLine("Dynamic Model Validation Details:\n");
            foreach (var detail in validationDetails)
            {
                Console.WriteLine($"{detail.Key}:");
                Console.WriteLine($" Type: {detail.Value.Type}");
                Console.WriteLine($" Constraint: {detail.Value.Constraints}");
                Console.WriteLine("------------------------------------------------------");
            }
        }

    }

}
