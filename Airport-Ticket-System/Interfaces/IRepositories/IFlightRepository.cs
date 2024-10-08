using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.Interfaces.IRepositories
{
    public interface IFlightRepository
    {
        IEnumerable<Flight> GetAllFlights();
        Flight GetFlightById(string flightId);
        void AddFlight(Flight flight);
        void RemoveFlight(string flightId);
        void UpdateFlight(Flight flight);
        void AddFlights(List<Flight> flights);
    }
}
