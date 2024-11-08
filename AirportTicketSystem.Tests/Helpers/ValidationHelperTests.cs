using Airport_Ticket_System.Helpers;

namespace AirportTicketSystem.Tests.Helpers
{
    public class ValidationHelperTests
    {
        private class SampleClass
        {
            [Validation("String", "Required")]
            public required string Name { get; set; }

            [Validation("Integer", "Required, Range(1,100)")]
            public int Age { get; set; }

            [Validation("Decimal", "Required, GreaterThan(0)")]
            public decimal Salary { get; set; }
        }

        [Fact]
        public void GetValidationDetails_ShouldReturnEmptyForClassWithoutAttributes()
        {
            var validationDetails = ValidationHelper.GetValidationDetails<object>();

            Assert.Empty(validationDetails);
        }
    }
}
