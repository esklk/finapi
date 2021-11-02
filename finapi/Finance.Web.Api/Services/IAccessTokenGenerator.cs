using System.Collections.Generic;
using System.Security.Claims;

namespace Finance.Web.Api.Services
{
    public interface IAccessTokenGenerator
    {
        string Generate(IEnumerable<Claim> claims);
    }
}
