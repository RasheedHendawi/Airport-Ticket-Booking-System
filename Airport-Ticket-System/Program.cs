using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Interfaces.IServices;
using Airport_Ticket_System.Models;
using Airport_Ticket_System.Utilites;
using Airport_Ticket_System.Utilites.ManagerUtilites;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            DisplayHelper.DisplayHeader();
            var services = ServiceFactory.CreateServices();
            var passenger = PassengerServiceHelper.GetPassengerInfo();

            var configManager = new ConfigManager();
            var (adminName, adminPassword) = configManager.GetAdminCredentials();

            if (IsAdminUser(passenger, adminName))
            {
                HandleAdminFlow(adminPassword);
            }
            else
            {
                HandlePassengerFlow(services, passenger);
            }

            Console.WriteLine("Thank you for using the Airport Ticket Booking System!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static bool IsAdminUser(Passenger passenger, string adminName)
    {
        return string.Equals(passenger.FirstName, adminName, StringComparison.OrdinalIgnoreCase);
    }

    private static void HandleAdminFlow(string adminPassword)
    {
        while (true)
        {
            Console.Write("Please enter the admin password (or type 'exit' to quit): ");
            var passKey = Console.ReadLine();

            if (string.Equals(passKey, "exit", StringComparison.OrdinalIgnoreCase))
                break;

            if (string.Equals(passKey, adminPassword))
            {

                ManagerMainMenu.ManagerMenu();
                break;
            }
            else
            {
                Console.WriteLine("Incorrect password. Please try again.");
            }
        }
    }

    private static void HandlePassengerFlow(
        (IPassengerService PassengerService, IFlightService FlightService, IBookingService BookingService) services,
        Passenger passenger)
    {
        bool continueApp = true;

        while (continueApp)
        {
            DisplayHelper.DisplayMainMenu();
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    PassengerServiceHelper.BookFlight(services.PassengerService, passenger);
                    break;

                case "2":
                    PassengerServiceHelper.SearchForFlights(services.PassengerService);
                    break;

                case "3":
                    PassengerServiceHelper.ManageBookings(services.PassengerService, passenger);
                    break;

                case "4":
                    continueApp = false;
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
