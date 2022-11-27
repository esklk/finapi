using System;

namespace Finance.Core.Configuration.Models
{
    public class DatabaseConfiguration
    {
        public string Database { get; set; }

        public string Password { get; set; }

        public string Port { get; set; }

        public string Server { get; set; }

        public Version ServerVersion { get; set; }

        public string UserId { get; set; }
    }
}
