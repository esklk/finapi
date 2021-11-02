namespace Finance.Web.Api.Services
{
    public interface ITokenValidatorFactory
    {
        ITokenValidator Create(string tokenProvider);
    }
}
