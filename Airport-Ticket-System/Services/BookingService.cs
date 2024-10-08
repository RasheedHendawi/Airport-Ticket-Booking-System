using Airport_Ticket_System.Exceptions.BookingExceptions;
using Airport_Ticket_System.Exceptions.FlightExceptions;
using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Interfaces.IServices;
using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;

        public BookingService(IBookingRepository bookingRepository, IFlightRepository flightRepository)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
        }

        public void BookFlight(string passengerId, string flightId,FlightClass flightClass)
        {
            var flight = _flightRepository.GetFlightById(flightId);
            if (flight == null)
            {
                throw new FlightNotFoundException(flightId);
            }
            
            var booking = new Booking
            {
                BookingId = Guid.NewGuid().ToString(),
                PassengerId = passengerId,
                FlightId = flightId,
                BookingDate = DateTime.Now,
                Class = flightClass,
                Price = FlightClassHelper.GetFlightPrice(flight.Price,flightClass)
            };

            _bookingRepository.AddBooking(booking);
        }

        public IEnumerable<Booking> GetBookingsByPassengerId(string passengerId)
        {
            return _bookingRepository.GetBookingByPassengerId(passengerId);
        }


        public void ModifyBooking(string bookingId, string newFlightId, FlightClass flightClass)
        {
            var booking = _bookingRepository.GetBookingById(bookingId);

            if (booking == null)
            {
                throw new BookingNotFoundException(bookingId);
            }
            booking.Class = flightClass;
            booking.Price = FlightClassHelper.GetFlightPrice(booking.Price, flightClass);
            booking.FlightId = newFlightId;
            booking.BookingDate = DateTime.Now;
            _bookingRepository.UpdateBooking(booking);
        }
        public void CancelBooking(string bookingId)
        {
            var booking = _bookingRepository.GetBookingById(bookingId);

            if (booking == null)
            {
                throw new BookingNotFoundException(bookingId);
            }

            _bookingRepository.RemoveBooking(bookingId);
        }

        public IEnumerable<Booking> FilterBookings(string? flightId = null,
                                                   decimal? maxPrice = null,
                                                   string? departureCountry = null,
                                                   string? destinationCountry = null,
                                                   DateTime? departureDate = null,
                                                   string? departureAirport = null,
                                                   string? arrivalAirport = null,
                                                   string? passengerId = null,
                                                   string? flightClass = null)
        {
            var bookings = _bookingRepository.GetAllBookings();
            var flights = _flightRepository.GetAllFlights();

            if (!string.IsNullOrEmpty(flightId))
                bookings = bookings.Where(b => b.FlightId.Equals(flightId, StringComparison.OrdinalIgnoreCase));

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

            if (!string.IsNullOrEmpty(passengerId))
                bookings = bookings.Where(b => b.PassengerId.Equals(passengerId, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(flightClass))
            {
                if (Enum.TryParse<FlightClass>(flightClass, true, out var parsedFlightClass))
                {
                    bookings = bookings.Where(f => f.Class == parsedFlightClass);
                }
                else
                {
                    Console.WriteLine($"Invalid flight class: {flightClass}. Please provide a valid value (Economy, Business, FirstClass).");
                }
            }
            var filteredFlightIds = flights.Select(f => f.FlightId).ToHashSet();
            bookings = bookings.Where(b => filteredFlightIds.Contains(b.FlightId));

            return bookings;
        }

        public void PrintBookings(IEnumerable<Booking> bookings)
        {
            if (bookings == null || !bookings.Any())
            {
                Console.WriteLine("You have no bookings.");
                return;
            }

            Console.WriteLine("Your Bookings:");
            Console.WriteLine("--------------------------------------------------------");

            foreach (var booking in bookings)
            {
                Console.WriteLine($"Booking ID:          {booking.BookingId}");
                Console.WriteLine($"Flight ID:           {booking.FlightId}");
                Console.WriteLine($"Class:               {booking.Class}");
                Console.WriteLine($"Booking Date:        {booking.BookingDate}");
                Console.WriteLine($"Price:               {booking.Price:C}");
                Console.WriteLine("--------------------------------------------------------");
            }

            Console.WriteLine($"Total Bookings: {bookings.Count()}");
        }


    }


}
