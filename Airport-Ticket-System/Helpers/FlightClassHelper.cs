using Airport_Ticket_System.Models;

namespace Airport_Ticket_System.Helpers
{
    public class FlightClassHelper
    {
        public static decimal GetFlightPrice(decimal basePrice, FlightClass flightClass)
        {
            switch (flightClass)
            {
                case FlightClass.Business:
                    return basePrice + (basePrice * 0.04m);
                case FlightClass.FirstClass:
                    return basePrice + (basePrice * 0.07m);
                case FlightClass.Economy:
                    return basePrice + (basePrice * 0.02m);
                default:
                    return basePrice;
            }
        }
    }
}
