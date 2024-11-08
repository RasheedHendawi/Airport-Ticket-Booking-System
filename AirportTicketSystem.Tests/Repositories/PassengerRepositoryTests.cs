using System;
using System.IO;
using System.Linq;
using Xunit;
using Airport_Ticket_System.Models;
using Airport_Ticket_System.RepositoriesHandler;

namespace AirportTicketSystem.Tests.Repositories
{
    public class PassengerRepositoryTests : IDisposable
    {
        private readonly string _tempFilePath;
        private PassengerRepository _repository;

        public PassengerRepositoryTests()
        {

            _tempFilePath = Path.Combine(Path.GetTempPath(), $"test_passengers_{Guid.NewGuid()}.csv");


            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }


            File.WriteAllText(_tempFilePath, "PassengerId;FirstName;LastName;Email;PhoneNumber\n");


            _repository = new PassengerRepository(_tempFilePath);
        }


        [Fact]
        public void AddPassenger_ShouldIncreasePassengerCountByOne()
        {

            var initialCount = _repository.GetAllPassengers().Count();
            var newPassenger = new Passenger
            {
                PassengerId = "P001",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890"
            };


            _repository.AddPassenger(newPassenger);
            var updatedCount = _repository.GetAllPassengers().Count();


            Assert.Equal(initialCount + 1, updatedCount);
        }

        [Fact]
        public void GetPassengerById_ShouldReturnCorrectPassenger()
        {

            var passenger = new Passenger
            {
                PassengerId = "P002",
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PhoneNumber = "987-654-3210"
            };
            _repository.AddPassenger(passenger);

            var result = _repository.GetPassengerById("P002");


            Assert.Equal("Jane", result.FirstName);
            Assert.Equal("Smith", result.LastName);
        }

        [Fact]
        public void AddPassenger_ShouldNotAffectOtherTests()
        {

            var passenger = new Passenger
            {
                PassengerId = "P003",
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                PhoneNumber = "321-654-9870"
            };

            _repository.AddPassenger(passenger);
            var allPassengers = _repository.GetAllPassengers();

            Assert.Contains(allPassengers, p => p.PassengerId == "P003");
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

            _repository = null!;
        }
    }
}
