using Airport_Ticket_System.Helpers;

namespace AirportTicketSystem.Tests.Helpers
{
    public class FilePathHelperTests
    {
        [Fact]
        public void GetDataFilePath_ShouldReturnCorrectPath_WhenTestFilePathIsNotSet()
        {
            FilePathHelper.SetTestFilePath(null!);
            var fileName = "bookings.csv";
            var expectedPath = Path.Combine(Directory.GetCurrentDirectory(), "DataStorage", fileName);


            var actualPath = FilePathHelper.GetDataFilePath(fileName);


            Assert.Equal(expectedPath, actualPath);
        }

        [Fact]
        public void GetDataFilePath_ShouldReturnTestFilePath_WhenSet()
        {

            var testFilePath = Path.Combine(Path.GetTempPath(), "test_bookings.csv");
            FilePathHelper.SetTestFilePath(testFilePath);
            var fileName = "bookings.csv"; 


            var actualPath = FilePathHelper.GetDataFilePath(fileName);


            Assert.Equal(testFilePath, actualPath);


            FilePathHelper.SetTestFilePath(null!);
        }
    }
}
