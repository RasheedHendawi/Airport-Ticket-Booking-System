﻿using Microsoft.Extensions.Configuration;

namespace Airport_Ticket_System.Helpers
{
    public class ConfigManager
    {
        public IConfiguration Configuration { get; }

        public ConfigManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public (string Username, string Password) GetAdminCredentials()
        {
            var username = Configuration["Admin:Username"];
            var password = Configuration["Admin:Password"];

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Admin credentials are not configured properly.");
            }

            return (username, password);
        }

    }

}
