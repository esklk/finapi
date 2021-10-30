using System;

namespace Finance.Web.Api.Configuration.Implementation
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; }

        public Version ServerVersion { get; set; }
    }
}
