using Airport_Ticket_System.Interfaces.IServices;
using Airport_Ticket_System.Models;
using Airport_Ticket_System.Services;

namespace Airport_Ticket_System.Helpers
{
    public class PassengerServiceHelper
    {
        public static Passenger GetPassengerInfo()
        {
            Console.Write("Enter First Name: ");
            var firstName = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            var lastName = Console.ReadLine();
            Console.Write("Enter Email: ");
            var email = Console.ReadLine();
            Console.Write("Enter Phone Number: ");
            var phoneNumber = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(firstName) &&
                !string.IsNullOrWhiteSpace(lastName) &&
                !string.IsNullOrWhiteSpace(email) &&
                !string.IsNullOrWhiteSpace(phoneNumber))
            {
                return PassengerService.GenaratePassenger(firstName, lastName, email, phoneNumber);
            }
            else
            {
                throw new ArgumentException("Null or empty input from user");
            }
        }


        public static void BookFlight(IPassengerService passengerService, Passenger passenger)
        {
            Console.WriteLine("Please enter the flight ID:");
            var flightId = Console.ReadLine();

            Console.WriteLine("Please select a flight class:");
            Console.WriteLine("1 - Economy");
            Console.WriteLine("2 - Business");
            Console.WriteLine("3 - First Class");

            FlightClass flightClass;

            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out int classOption) && classOption >= 1 && classOption <= 3)
                {
                    flightClass = classOption switch
                    {
                        1 => FlightClass.Economy,
                        2 => FlightClass.Business,
                        3 => FlightClass.FirstClass,
                        _ => FlightClass.Economy
                    };
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 1, 2, or 3 for the flight class.");
                }
            }
            if (flightId != null)
            {
                passengerService.BookFlight(passenger.PassengerId, flightId, flightClass);
                Console.WriteLine("Flight booked successfully!");
            }
            else
            {
                throw new Exception("flight ID null");
            }
        }


        public static void SearchForFlights(IPassengerService passengerService)
        {
            passengerService.PrintAllFlights();
            Console.WriteLine("Do you want to filter the results? (yes/no)");
            var result = Console.ReadLine();
            result ??= "no";
            result=result.ToLower();
            if (result == "yes")
                {
                    Console.WriteLine("Enter Departure Country (leave blank to skip):");
                    var departureCountry = Console.ReadLine();

                    Console.WriteLine("Enter Destination Country (leave blank to skip):");
                    var destinationCountry = Console.ReadLine();

                    Console.WriteLine("Enter Departure Date (yyyy-mm-dd, leave blank to skip):");
                    var departureDateInput = Console.ReadLine();
                    DateTime? departureDate = null;
                    if (DateTime.TryParse(departureDateInput, out var parsedDate))
                    {
                        departureDate = parsedDate;
                    }

                    Console.WriteLine("Enter Departure Airport (leave blank to skip):");
                    var departureAirport = Console.ReadLine();

                    Console.WriteLine("Enter Arrival Airport (leave blank to skip):");
                    var arrivalAirport = Console.ReadLine();

                    Console.WriteLine("Enter Maximum Price (leave blank to skip):");
                    var maxPriceInput = Console.ReadLine();
                    decimal? maxPrice = null;
                    if (decimal.TryParse(maxPriceInput, out var parsedPrice))
                    {
                        maxPrice = parsedPrice;
                    }

                    var filteredFlights = passengerService.SearchAvailableFlights(maxPrice, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport);

                    if (!filteredFlights.Any())
                    {
                        Console.WriteLine("\nNo flights match the given criteria.");
                    }
                    else
                    {
                        Console.WriteLine("\nFiltered Flights:");
                        Console.WriteLine("--------------------------------------------------------");
                        foreach (var flight in filteredFlights)
                        {
                            Console.WriteLine($"Flight ID: {flight.FlightId}");
                            Console.WriteLine($"Departure Country: {flight.DepartureCountry}");
                            Console.WriteLine($"Destination Country: {flight.DestinationCountry}");
                            Console.WriteLine($"Departure Date: {flight.DepartureDate:yyyy-MM-dd}");
                            Console.WriteLine($"Departure Airport: {flight.DepartureAirport}");
                            Console.WriteLine($"Arrival Airport: {flight.ArrivalAirport}");
                            Console.WriteLine($"Price: {flight.Price:C}");
                            Console.WriteLine("--------------------------------------------------------");
                        }
                    }
                }
        }

        public static void ManageBookings(IPassengerService passengerService, Passenger passenger)
        {
            while (true)
            {
                Console.WriteLine("Manage Bookings:");
                Console.WriteLine("1. View My Bookings");
                Console.WriteLine("2. Cancel a Booking");
                Console.WriteLine("3. Modify a Booking");
                Console.WriteLine("4. Exit");

                Console.Write("Select an option (1-4): ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ViewBookings(passengerService, passenger.PassengerId);
                        break;
                    case "2":
                        CancelBooking(passengerService, passenger.PassengerId);
                        break;
                    case "3":
                        ModifyBooking(passengerService, passenger.PassengerId);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public static void ViewBookings(IPassengerService passengerService, string passengerId)
        {
            var bookings = passengerService.ViewBookings(passengerId);
            if (!bookings.Any())
            {
                Console.WriteLine("You have no bookings.");
            }
            else
            {
                Console.WriteLine("Your Bookings:");
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"Booking ID:          {booking.BookingId}");
                    Console.WriteLine($"Flight ID:           {booking.FlightId}");
                    Console.WriteLine($" Class:              {booking.Class}");
                    Console.WriteLine($"Booking Date:        {booking.BookingDate}");
                    Console.WriteLine($"Price:               {booking.Price:C}");
                    Console.WriteLine("--------------------------------------------------------");
                }
            }
        }

        public static void CancelBooking(IPassengerService passengerService, string passengerId)
        {
            Console.Write("Enter the Booking ID you want to cancel: ");
            var bookingId = Console.ReadLine();

            try
            {
                if (passengerId != null && bookingId != null)
                {
                    passengerService.CancelBooking(passengerId, bookingId);
                }
                else 
                {
                    throw new Exception("PassegnerServicehelper null inputs");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void ModifyBooking(IPassengerService passengerService, string passengerId)
        {
            Console.Write("Enter the Booking ID you want to modify: ");
            var bookingId = Console.ReadLine();


            var currentBooking = passengerService.ViewBookings(passengerId).FirstOrDefault(b => b.BookingId == bookingId);
            if (currentBooking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            Console.WriteLine($"Current Flight ID: {currentBooking.FlightId}, Class: {currentBooking.Class}, Price: {currentBooking.Price:C}");
            Console.Write("Enter the new Flight ID: ");
            var newFlightId = Console.ReadLine();

            Console.WriteLine("Select new Flight Class (Economy, Business, FirstClass): ");
            var newClassInput = Console.ReadLine();

            if (Enum.TryParse<FlightClass>(newClassInput, true, out var newClass))
            {
                try
                {
                    if (bookingId != null && newFlightId != null)
                    {
                        passengerService.ModifyBooking(passengerId, bookingId, newFlightId, newClass);
                        Console.WriteLine("Booking modified successfully!");
                    }
                    else throw new Exception("error happend while taking flight||booking IDs");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid flight class. Please enter a valid class (Economy, Business, FirstClass).");
            }
        }
    }
}
