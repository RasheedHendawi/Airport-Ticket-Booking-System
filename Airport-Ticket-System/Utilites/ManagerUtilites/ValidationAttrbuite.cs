using System;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ValidationAttribute : Attribute
{
    public string Type { get; }
    public string Constraints { get; }

    public ValidationAttribute(string type, string constraints)
    {
        Type = type;
        Constraints = constraints;
    }
}
