using Finance.Web.Api.Configuration.Implementation;
using System;

namespace Finance.Web.Api.Services
{
    public interface IOauthConfigurationProvider
    {
        OAuthConfiguration GetRelated(Type relatedType);
    }
}
