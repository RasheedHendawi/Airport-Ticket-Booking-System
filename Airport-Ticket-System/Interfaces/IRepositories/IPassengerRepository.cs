using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.Interfaces.IRepositories
{
    public interface IPassengerRepository
    {
        IEnumerable<Passenger> GetAllPassengers();
        Passenger GetPassengerById(string passengerId);
        void AddPassenger(Passenger passenger);
        void RemovePassenger(string passengerId);
        void UpdatePassenger(Passenger passenger);
    }
}
