using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Finance.Web.Api.Controllers
{
    [Route("/123/")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private int _currentUserId = -1;
        protected int CurrentUserId
        {
            get
            {
                if (_currentUserId < 1
                    && User.Identity.IsAuthenticated
                    && int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int value))
                {
                    _currentUserId = value;
                }

                return _currentUserId;
            }
        }
    }
}
