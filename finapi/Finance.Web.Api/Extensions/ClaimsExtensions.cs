using System.Security.Claims;

namespace Finance.Web.Api.Extensions
{
    public static class ClaimsExtensions
    {
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (int.TryParse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), out int value))
            {
                return value;
            }

            return default;
        }
    }
}
