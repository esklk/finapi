using System.Collections.Generic;
using System.Security.Claims;

namespace Finance.Web.Api.Services
{
    public interface ITokenValidator
    {
        IEnumerable<Claim> Validate(string token);
    }
}
