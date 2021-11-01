using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Web.Api.Models
{
    public class LoginModel : List<LoginOptionModel>
    {
        public LoginModel(IEnumerable<LoginOptionModel> options) : base(options)
        {

        }

        public string RedirectUrl { get; set; }
    }
}
