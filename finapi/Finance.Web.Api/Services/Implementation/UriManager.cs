using System;
using System.Collections.Specialized;
using System.Web;

namespace Finance.Web.Api.Services.Implementation
{
    public class UriManager : IUriManager
    {
        public string SetQueryParamaters(string uri, params (string name, string value)[] parameters)
        {
            var uriBuilder = new UriBuilder(uri);
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var parameter in parameters)
            {
                query[parameter.name] = parameter.value;
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }
    }
}
