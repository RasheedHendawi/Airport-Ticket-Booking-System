using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Interfaces.IServices;
using Airport_Ticket_System.RepositoriesHandler;

namespace Airport_Ticket_System.Utilites.ManagerUtilites
{
    public class ManagerMainMenu
    {
        public static void ManagerMenu()
        {
            bool continueManagerMenu = true;

            while (continueManagerMenu)
            {
                var services = ServiceFactory.CreateServices();
                //Console.Clear();
                Console.WriteLine("Welcome to the Manager Menu!");
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1: Import Flights from CSV");
                Console.WriteLine("2: View Flight Data");
                Console.WriteLine("3: View Bookings");
                Console.WriteLine("4: Exit");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ImportFlights(services.FlightService);
                        break;

                    case "2":
                         services.FlightService.PrintValidationDetails();
                        break;

                    case "3":
                        SearchForBooking(services.BookingService);
                        break;

                    case "4":
                        continueManagerMenu = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }

            Console.WriteLine("Thank you for using the Manager Menu!");
        }

        private static void ImportFlights(IFlightService flightService)
        {
            Console.WriteLine("Enter the path to the CSV file to import flights:");
            var filePath = Console.ReadLine();

            try
            {
                if (filePath != null)
                {
                    flightService.ImportFlightsFromCsv(filePath);
                }
                else
                {
                    throw new Exception("Empty file path");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while importing flights: {ex.Message}");
            }
        }
        private static void SearchForBooking(IBookingService bookingService)
        {
            try
            {
                var bookingRepo = new BookingRepository("bookings.csv");
                bookingService.PrintBookings(bookingRepo.GetAllBookings());

                Console.WriteLine("Do you want to filter the results? (yes/no)");
                var result = Console.ReadLine();
                if (result == null) result = "no";
                else 
                {
                    result=result.ToLower();
                }
                if (result == "yes")
                {
                    Console.WriteLine("Enter Flight ID (leave blank to skip):");
                    var flightId = Console.ReadLine();

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

                    Console.WriteLine("Enter Passenger ID (leave blank to skip):");
                    var passengerId = Console.ReadLine();

                    Console.WriteLine("Enter Flight Class (Economy, Business, FirstClass, leave blank to skip):");
                    var flightClass = Console.ReadLine();

                    var filteredBookings = bookingService.FilterBookings(
                        flightId,
                        maxPrice,
                        departureCountry,
                        destinationCountry,
                        departureDate,
                        departureAirport,
                        arrivalAirport,
                        passengerId,
                        flightClass
                    );
                    bookingService.PrintBookings(filteredBookings);
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"Error happend when answering yes/no{ex.Message}");
            }

        }


    }
}
