using Finance.Web.Api.Models;

namespace Finance.Web.Api.Services
{
    public interface ILoginService
    {
        LoginOptionModel[] GetLoginOptionModels();
    }
}
