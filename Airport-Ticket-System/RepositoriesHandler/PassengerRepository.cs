using Airport_Ticket_System.Models;
using Airport_Ticket_System.Exceptions.PassengerExceptions;
using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Interfaces.IRepositories;

namespace Airport_Ticket_System.RepositoriesHandler
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly List<Passenger> _passengers;
        private readonly string _filePath;

        public PassengerRepository(string filePath)
        {
            _filePath = FilePathHelper.GetDataFilePath(filePath);
            _passengers = LoadPassengersFromFile();
        }

        private List<Passenger> LoadPassengersFromFile()
        {
            if (!File.Exists(_filePath))
                return [];

            using (var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(fileStream))
            {
                var lines = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                return lines.Skip(1)
                            .Select(CsvToPassenger)
                            .ToList();
            }
        }

        private static Passenger CsvToPassenger(string csvLine)
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
            using (var fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (var writer = new StreamWriter(fileStream))
            {
                writer.WriteLine("PassengerId;FirstName;LastName;Email;PhoneNumber");
                foreach (var passenger in _passengers)
                {
                    writer.WriteLine($"{passenger.PassengerId};{passenger.FirstName};{passenger.LastName};{passenger.Email};{passenger.PhoneNumber}");
                }
            }
        }

        public IEnumerable<Passenger> GetAllPassengers()
        {
            return _passengers;
        }

        public Passenger GetPassengerById(string passengerId)
        {
            var passenger = _passengers.FirstOrDefault(p => p.PassengerId == passengerId);
            return passenger ?? throw new PassengerNotFoundException(passengerId);
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
