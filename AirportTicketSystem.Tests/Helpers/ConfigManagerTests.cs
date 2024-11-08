using Xunit;
using Microsoft.Extensions.Configuration;
using Airport_Ticket_System.Helpers;
using System.IO;
using System;

namespace AirportTicketSystem.Tests.Helpers
{
    public class ConfigManagerTests
    {
        [Fact]
        public void GetAdminCredentials_ShouldReturnCredentials()
        {
            var configManager = new ConfigManager();

            var credentials = configManager.GetAdminCredentials();

            Assert.False(string.IsNullOrEmpty(credentials.Username), "Username should not be empty.");
            Assert.False(string.IsNullOrEmpty(credentials.Password), "Password should not be empty.");
        }
    }
}
