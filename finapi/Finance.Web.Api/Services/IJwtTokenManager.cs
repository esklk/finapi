using System.Collections.Generic;

namespace Finance.Web.Api.Services
{
    public interface IJwtTokenManager
    {
        Dictionary<string, string> GetPayload(string token);
    }
}
