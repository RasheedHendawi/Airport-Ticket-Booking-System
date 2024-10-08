using Airport_Ticket_System.Exceptions.BookingExceptions;
using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private List<Booking> _bookings;
        private readonly string _filePath;

        public BookingRepository()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "DataStorage", "bookings.csv");
            _bookings = LoadBookingsFromFile();
        }


        private List<Booking> LoadBookingsFromFile()
        {
            if (!File.Exists(_filePath))
                return new List<Booking>();

            return File.ReadAllLines(_filePath)
                       .Skip(1) 
                       .Select(line => CsvToBooking(line))
                       .ToList();
        }

        private Booking CsvToBooking(string csvLine)
        {
            var values = csvLine.Split(';');
            return new Booking
            {
                BookingId = values[0],
                PassengerId = values[1],
                FlightId = values[2],
                BookingDate = DateTime.Parse(values[3]),
                Class = Enum.Parse<FlightClass>(values[4]),
                Price = Decimal.Parse(values[5])
            };
        }


        private void SaveBookingsToFile()
        {
            var lines = new List<string>
            {
                 "BookingId;PassengerId;FlightId;BookingDate;FlightClass;Price"
            };

            lines.AddRange(_bookings.Select(b => $"{b.BookingId};{b.PassengerId};{b.FlightId};{b.BookingDate};{b.Class};{b.Price}"));
            File.WriteAllLines(_filePath, lines);
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            return _bookings;
        }

        public Booking GetBookingById(string bookingId)
        {
            var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null)
                throw new BookingNotFoundException(bookingId);
            return booking;
        }
        public void AddBooking(Booking booking)
        {
            _bookings.Add(booking);
            SaveBookingsToFile();
        }

        public void RemoveBooking(string bookingId)
        {
            var booking = GetBookingById(bookingId);
            _bookings.Remove(booking);
            SaveBookingsToFile();
        }

        public void UpdateBooking(Booking booking)
        {
            var existingBooking = GetBookingById(booking.BookingId);
            _bookings[_bookings.IndexOf(existingBooking)] = booking;
            SaveBookingsToFile();
        }

        public IEnumerable<Booking> GetBookingByPassengerId(string passengerId)
        {
            return _bookings.Where(b => b.PassengerId == passengerId).ToList();
        }
    }
}

