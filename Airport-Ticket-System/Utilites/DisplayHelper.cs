
namespace Airport_Ticket_System.Utilites
{
    internal class DisplayHelper
    {
        public static void DisplayHeader()
        {
            var airPlane = """"""
                    
                                                 |                    
                                           --====|====--
                                                 |  

                                             .-"""""-. 
                                           .'_________'. 
                                          /_/_|__|__|_\_\
                                         ;'-._       _.-';
                    ,--------------------|    `-. .-'    |--------------------,
                     ``""--..__    ___   ;       '       ;   ___    __..--""``
                               `"-// \\.._\             /_..// \\-"`
                                  \\_//    '._       _.'    \\_//
                                   `"`        ``---``        `"`

                    """""";
            Console.WriteLine(airPlane);
            Console.WriteLine("\tWelcome to the Airport Ticket Booking System!\n");
        }
        public static void DisplayMainMenu()
        {
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Book a flight");
            Console.WriteLine("2. Search for flights");
            Console.WriteLine("3. Manage my bookings");
            Console.WriteLine("4. Exit");
        }
    }
}
