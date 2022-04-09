using Finance.Access.Models;

namespace Finance.Access.Services
{
    public interface IAccessDataProvider
    {
        public AccessDataModel GetData();
    }
}
