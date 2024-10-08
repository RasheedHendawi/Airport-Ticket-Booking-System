using Airport_Ticket_System.Helpers;
using Airport_Ticket_System.Utilites;
using Airport_Ticket_System.Utilites.ManagerUtilites;
class Program
{
    static void Main(string[] args)
    {
        DisplayHelper.DisplayHeader();
        var services = ServiceFactory.CreateServices();
        var passengerServiceHelper = new PassengerServiceHelper();
        var passenger = passengerServiceHelper.GetPassengerInfo();
        if (passenger.FirstName.Equals("Admin"))
        {
            var manager = new ManagerMainMenu();
            manager.ManagerMenu();
        }
        else
        {
            bool continueApp = true;
            while (continueApp)
            {
                DisplayHelper.DisplayMainMenu();
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        passengerServiceHelper.BookFlight(services.PassengerService, passenger);
                        break;

                    case "2":
                        passengerServiceHelper.SearchForFlights(services.PassengerService);
                        break;

                    case "3":
                        passengerServiceHelper.ManageBookings(services.PassengerService, passenger);
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

        Console.WriteLine("Thank you for using the Airport Ticket Booking System!");
    }




}
