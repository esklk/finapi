namespace Finance.Web.Api.Services
{
    public interface IUriManager
    {
        string SetQueryParamaters(string uri, params (string name, string value)[] parameters);
    }
}
