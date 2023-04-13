namespace Finance.Web.Api.Configuration
{
    public record TokenConfiguration
    {
        public JwtConfiguration Access { get; set; }
        public JwtConfiguration Refresh { get; set; }
    }
}
