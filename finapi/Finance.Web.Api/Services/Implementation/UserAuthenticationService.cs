using Finance.Business.Models;
using Finance.Business.Services;
using Finance.Web.Api.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Finance.Web.Api.Services.Implementation
{
    public class UserAuthenticationService : IAuthenticationService<UserAuthenticationData>
    {
        private readonly IUserLoginService _userLoginService;
        private readonly IUserService _userService;
        private readonly ITokenGenerator _accessTokenGenerator;
        private readonly ITokenGenerator _refreshTokenGenerator;

        public UserAuthenticationService(IUserLoginService userLoginService, IUserService userService, ITokenGenerator accessTokenGenerator, ITokenGenerator refreshTokenGenerator)
        {
            _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _accessTokenGenerator = accessTokenGenerator ?? throw new ArgumentNullException(nameof(accessTokenGenerator));
            _refreshTokenGenerator = refreshTokenGenerator ?? throw new ArgumentNullException(nameof(refreshTokenGenerator));
        }

        public async Task<AuthModel> AuthenticateAsync(UserAuthenticationData data)
        {
            UserModel user;
            var userLogin = await _userLoginService.GetUserLoginAsync(data.LoginProvider, data.LoginIdentifier);
            if (userLogin == null)
            {
                user = await _userService.CreateUserAsync(new UserModel(data.Name));
                userLogin = await _userLoginService.CreateUserLoginAsync(user.Id, data.LoginProvider, data.LoginIdentifier);
            }
            else
            {
                user = await _userService.GetUserAsync(userLogin.UserId);
            }

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

            return new AuthModel()
            {
                AccessToken = _accessTokenGenerator.Generate(claims),
                RefreshToken = _refreshTokenGenerator.Generate(claims)
            };
        }
    }
}
