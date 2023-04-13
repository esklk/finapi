using System.Collections.Generic;
using System.Security.Claims;

namespace Finance.Web.Api.Services
{
    public interface ITokenGenerator
    {
        string Generate(IEnumerable<Claim> claims);
    }
}
