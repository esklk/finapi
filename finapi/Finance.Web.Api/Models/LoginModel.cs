using System.Collections.Generic;

namespace Finance.Web.Api.Models
{
    public class LoginModel : List<LoginOptionModel>
    {
        public LoginModel(IEnumerable<LoginOptionModel> options) : base(options) { }

        public string Token { get; set; }
    }
}
