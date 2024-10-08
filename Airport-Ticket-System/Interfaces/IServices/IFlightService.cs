using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.Interfaces.IServices
{
    public interface IFlightService
    {
        public IEnumerable<Flight> SearchFlights(
            string departureCountry,
            string destinationCountry,
            DateTime? departureDate,
            string departureAirport,
            string arrivalAirport,
            FlightClass? flightClass,
            decimal? maxPrice);
        public void ImportFlightsFromCsv(string filePath);
        void PrintValidationDetails();
    }
}
