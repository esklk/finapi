namespace Finance.Web.Api.Models
{
    public record UserAuthenticationData
    {
        public string LoginIdentifier { get; set; }

        public string LoginProvider { get; set; }

        public string Name { get; set; }
    }
}
