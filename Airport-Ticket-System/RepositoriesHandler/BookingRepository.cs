using Airport_Ticket_System.Exceptions.BookingExceptions;
using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Interfaces.IRepositories;
using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.RepositoriesHandler
{
    public class BookingRepository : IBookingRepository
    {
        private readonly List<Booking> _bookings;
        private readonly string _filePath;

        public BookingRepository(string filepath)
        {
            _filePath = FilePathHelper.GetDataFilePath(filepath);
            _bookings = LoadBookingsFromFile();
        }


        private List<Booking> LoadBookingsFromFile()
        {
            if (!File.Exists(_filePath))
                return [];

            return File.ReadAllLines(_filePath)
                       .Skip(1)
                       .Select(line => CsvToBooking(line))
                       .ToList();
        }

        private static Booking CsvToBooking(string csvLine)
        {
            var values = csvLine.Split(';');
            return new Booking
            {
                BookingId = values[0],
                PassengerId = values[1],
                FlightId = values[2],
                BookingDate = DateTime.Parse(values[3]),
                Class = Enum.Parse<FlightClass>(values[4]),
                Price = decimal.Parse(values[5])
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
            return booking ?? throw new BookingNotFoundException(bookingId);
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

