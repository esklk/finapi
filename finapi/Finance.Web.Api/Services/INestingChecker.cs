using System.Threading.Tasks;

namespace Finance.Web.Api.Services
{
    public interface INestingChecker
    {
        Task<bool> IsResourceNestedToParentAsync(int resourceId, int parentId);
    }
}
