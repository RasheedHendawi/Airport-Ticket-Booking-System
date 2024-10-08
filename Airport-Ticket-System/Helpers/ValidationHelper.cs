using System.Reflection;

namespace Airport_Ticket_System.Helpers
{
    public class ValidationHelper
    {
        public static Dictionary<string, (string Type, string Constraints)> GetValidationDetails<T>()
        {
            var validationDetails = new Dictionary<string, (string Type, string Constraints)>();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var validationAttribute = property.GetCustomAttribute<ValidationAttribute>();
                if (validationAttribute != null)
                {
                    validationDetails[property.Name] = (validationAttribute.Type, validationAttribute.Constraints);
                }
            }

            return validationDetails;
        }
    }
}