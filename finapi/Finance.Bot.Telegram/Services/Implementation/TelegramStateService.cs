using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Business.Models;
using Finance.Business.Services;
using Telegram.Bot.Types;

namespace Finance.Bot.Telegram.Services.Implementation
{
    internal class TelegramStateService : IStateService
    {
        private const string TelegramLoginProvider = "Telegram";

        private readonly IStateService _internalStateService;
        private readonly IUserService _userService;
        private readonly IUserLoginService _userLoginService;
        private readonly User _telegramUser;
        private readonly Type _initialProcessorType;

        public TelegramStateService(IStateService internalStateService, IUserService userService, IUserLoginService userLoginService, User telegramUser, Type initialProcessorType)
        {
            _internalStateService = internalStateService ?? throw new ArgumentNullException(nameof(internalStateService));
            _userService = userService;
            _userLoginService = userLoginService;
            _telegramUser = telegramUser ?? throw new ArgumentNullException(nameof(telegramUser));
            _initialProcessorType = initialProcessorType ?? throw new ArgumentNullException(nameof(initialProcessorType));
        }

        public async Task<State?> GetStateAsync()
        {
            State? internalState = await _internalStateService.GetStateAsync();

            if (internalState != null)
            {
                return internalState;
            }
            
            UserLoginModel? userLogin = await _userLoginService.GetUserLoginAsync(TelegramLoginProvider, _telegramUser.Id.ToString());
            if (userLogin != null)
            {
                return new State(_initialProcessorType,
                    new Dictionary<string, string> { { "UserId", userLogin.UserId.ToString() } });
            }

            UserModel user = await _userService.CreateUserAsync(new UserModel(_telegramUser.FirstName));
            userLogin = await _userLoginService.CreateUserLoginAsync(user.Id, TelegramLoginProvider,
                _telegramUser.Id.ToString());

            return new State(_initialProcessorType,
                new Dictionary<string, string> { { "UserId", userLogin.UserId.ToString() } });

        }

        public async Task SetStateAsync(State state)
        {
            await _internalStateService.SetStateAsync(state);
        }
    }
}
