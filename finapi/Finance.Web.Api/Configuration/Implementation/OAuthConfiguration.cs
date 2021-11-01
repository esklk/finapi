using System.Collections.Generic;

namespace Finance.Web.Api.Configuration.Implementation
{
    public class OAuthConfiguration
    {
        public string Secret { get; set; }

        public string Endpoint { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }
}
