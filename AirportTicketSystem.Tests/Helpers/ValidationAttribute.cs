using System;

namespace AirportTicketSystem.Tests.Helpers
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ValidationAttribute(string type, string constraints) : Attribute
    {
        public string Type { get; } = type;
        public string Constraints { get; } = constraints;
    }
}