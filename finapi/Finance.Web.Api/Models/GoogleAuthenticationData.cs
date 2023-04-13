namespace Finance.Web.Api.Models
{
    public record GoogleAuthenticationData
    {
        public string ClientId { get; set; }

        public string Credential { get; set; }
    }
}
