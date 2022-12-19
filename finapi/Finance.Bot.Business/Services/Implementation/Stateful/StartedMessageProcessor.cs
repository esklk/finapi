﻿using Finance.Bot.Business.Models;
using Finance.Business.Models;
using Finance.Business.Services;

namespace Finance.Bot.Business.Services.Implementation.Stateful
{
    public abstract class StartedMessageProcessor : IStatefulMessageProcessor
    {
        private const string WelcomeMessage =
            $"Hello, I'm Finn. I will help you manage your finances. To start you must select an account. Don't have one yet? Create it with \"{Commands.CreateAccount}\"! You can also use \"{Commands.Help}\" to learn how to get the most using all existing features. Have fun!";

        private readonly IUserService _userService;
        private readonly IUserLoginService _userLoginService;

        public StartedMessageProcessor(IUserService userService, IUserLoginService userLoginService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
        }

        public async Task<MessageResponse> ProcessAsync(State state, string? text)
        {
            if (!state.ContainsKey(StateKeys.UserId))
            {
                UserLoginModel? userLogin = await _userLoginService.GetUserLoginAsync(LoginProvider, LoginIdentifier);
                if (userLogin == null)
                {
                    UserModel user = await _userService.CreateUserAsync(new UserModel(UserFirstName));
                    userLogin = await _userLoginService.CreateUserLoginAsync(user.Id, LoginProvider,
                        LoginIdentifier);
                }

                state[StateKeys.UserId] = userLogin.UserId;
            }

            state.ProcessorType = typeof(SignedInMessageProcessor);

            return new MessageResponse(WelcomeMessage, Commands.CreateAccount, Commands.Help);
        }

        protected abstract string UserFirstName { get; }

        protected abstract string LoginIdentifier { get; }
        protected abstract string LoginProvider { get; }
    }
}
