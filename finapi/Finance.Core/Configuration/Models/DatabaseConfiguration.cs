using System;

namespace Finance.Core.Configuration.Models
{
    public class DatabaseConfiguration
    {
        public string Database { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string Port { get; set; } = default!;

        public string Server { get; set; } = default!;

        public Version ServerVersion { get; set; } = default!;

        public string UserId { get; set; } = default!;
    }
}
