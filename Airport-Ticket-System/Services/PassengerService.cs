using Airport_Ticket_System.Exceptions.FlightExceptions;
using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Interfaces.IServices;
using Airport_Ticket_System.Models;
using Airport_Ticket_System.RepositoriesHandler;

namespace Airport_Ticket_System.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IBookingRepository _bookingRepository;

        public PassengerService(IFlightRepository flightRepository, IBookingRepository bookingRepository)
        {
            _flightRepository = flightRepository;
            _bookingRepository = bookingRepository;
        }


        public static Passenger GenaratePassenger(string firstName, string lastName, string email, string phoneNumber)
        {
            var passengerRepository = new PassengerRepository("passengers.csv");

            Passenger passenger = new()
            {
                PassengerId = Guid.NewGuid().ToString(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber
            };
            passengerRepository.AddPassenger(passenger);
            return passenger;
        }
        public IEnumerable<Flight> SearchAvailableFlights(decimal? maxPrice = null,
                                                          string? departureCountry = null,
                                                          string? destinationCountry = null,
                                                          DateTime? departureDate = null,
                                                          string? departureAirport = null,
                                                          string? arrivalAirport = null,
                                                          string? flightClass = null)
        {
            var flights = _flightRepository.GetAllFlights();

            if (maxPrice.HasValue)
                flights = flights.Where(f => f.Price <= maxPrice.Value);

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

            return flights;
        }


        public void BookFlight(string passengerId, string flightId, FlightClass flightClass)
        {
            var bookRepository = _bookingRepository;
            var flightRepository = _flightRepository;
            var booking = new BookingService(bookRepository, flightRepository);
            booking.BookFlight(passengerId, flightId, flightClass);
        }

        public void PrintAllFlights()
        {
            var flights = _flightRepository.GetAllFlights();

            if (!flights.Any())
            {
                Console.WriteLine("No flights available at the moment.");
                return;
            }

            Console.WriteLine("\nAvailable Flights:");
            Console.WriteLine("--------------------------------------------------------");

            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight ID:           {flight.FlightId}");
                Console.WriteLine($"Departure Country:   {flight.DepartureCountry}");
                Console.WriteLine($"Destination Country: {flight.DestinationCountry}");
                Console.WriteLine($"Departure Date:      {flight.DepartureDate:yyyy-MM-dd}");
                Console.WriteLine($"Departure Airport:   {flight.DepartureAirport}");
                Console.WriteLine($"Arrival Airport:     {flight.ArrivalAirport}");
                Console.WriteLine($"Price:               {flight.Price:C}");
                Console.WriteLine("--------------------------------------------------------");
            }
        }

        public IEnumerable<Booking> ViewBookings(string passengerId)
        {
            return _bookingRepository.GetBookingByPassengerId(passengerId);
        }


        public void CancelBooking(string passengerId, string bookingId)
        {
            var booking = _bookingRepository.GetBookingById(bookingId);
            if (booking == null || booking.PassengerId != passengerId)
                throw new ArgumentException("Booking not found or you don't have permission to cancel this booking.");

            _bookingRepository.RemoveBooking(bookingId);
            Console.WriteLine("Booking canceled successfully.");
        }

        public void ModifyBooking(string passengerId, string bookingId, string flightId, FlightClass newClass)
        {
            var booking = _bookingRepository.GetBookingById(bookingId);
            if (booking == null || booking.PassengerId != passengerId)
                throw new ArgumentException("Booking not found or you don't have permission to modify this booking.");

            if (_flightRepository.GetFlightById(flightId) == null)
                throw new FlightNotFoundException(flightId);

            booking.Class = newClass;
            booking.FlightId = flightId;
            booking.Price = FlightClassHelper.GetFlightPrice(booking.Price, newClass);
            _bookingRepository.UpdateBooking(booking);
        }
    }

}