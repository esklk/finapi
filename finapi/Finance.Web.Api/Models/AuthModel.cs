namespace Finance.Web.Api.Models
{
    public record AuthModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
