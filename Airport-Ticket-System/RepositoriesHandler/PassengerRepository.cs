using Airport_Ticket_System.Exceptions;
using Airport_Ticket_System.Exceptions.PassengerExceptions;
using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private List<Passenger> _passengers;
        private readonly string _filePath;

        public PassengerRepository()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "DataStorage", "passengers.csv");
            _passengers = LoadPassengersFromFile();
        }


        private List<Passenger> LoadPassengersFromFile()
        {
            if (!File.Exists(_filePath))
                return new List<Passenger>();

            return File.ReadAllLines(_filePath)
                       .Skip(1) 
                       .Select(line => CsvToPassenger(line))
                       .ToList();
        }


        private Passenger CsvToPassenger(string csvLine)
        {
            var values = csvLine.Split(';');
            return new Passenger
            {
                PassengerId = values[0],
                FirstName = values[1],
                LastName = values[2],
                Email = values[3],
                PhoneNumber = values[4]
            };
        }


        private void SavePassengersToFile()
        {
            var lines = new List<string>
        {
            "PassengerId;FirstName;LastName;Email;PhoneNumber"
        };

            lines.AddRange(_passengers.Select(p => $"{p.PassengerId};{p.FirstName};{p.LastName};{p.Email};{p.PhoneNumber}"));
            File.WriteAllLines(_filePath, lines);
        }

        public IEnumerable<Passenger> GetAllPassengers()
        {
            return _passengers;
        }

        public Passenger GetPassengerById(string passengerId)
        {
            var passenger = _passengers.FirstOrDefault(p => p.PassengerId == passengerId);
            if (passenger == null)
                throw new PassengerNotFoundException(passengerId);
            return passenger;
        }

        public void AddPassenger(Passenger passenger)
        {
            _passengers.Add(passenger);
            SavePassengersToFile(); 
        }

        public void RemovePassenger(string passengerId)
        {
            var passenger = GetPassengerById(passengerId);
            _passengers.Remove(passenger);
            SavePassengersToFile();
        }

        public void UpdatePassenger(Passenger passenger)
        {
            var existingPassenger = GetPassengerById(passenger.PassengerId);
            _passengers[_passengers.IndexOf(existingPassenger)] = passenger;
            SavePassengersToFile();
        }
    }

}
