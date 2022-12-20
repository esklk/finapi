using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Business.Models;
using Finance.Business.Services;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class Start : IBotCommand
    {
        private const string WelcomeMessage =
            "Hello, I'm Finn. I will help you manage your finances. To start you must select an account.";

        private readonly IUserService _userService;
        private readonly IUserLoginService _userLoginService;
        private readonly IBotMessageSender _botMessageSender;
        private readonly string _firstName;
        private readonly string _loginIdentifier;
        private readonly string _loginProvider;

        public Start(IUserService userService, 
            IUserLoginService userLoginService, 
            IBotMessageSender botMessageSender,
            string firstName,
            string loginIdentifier,
            string loginProvider)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(firstName));
            }

            if (string.IsNullOrWhiteSpace(loginIdentifier))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(loginIdentifier));
            }

            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(loginProvider));
            }

            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
            _botMessageSender = botMessageSender ?? throw new ArgumentNullException(nameof(botMessageSender));
            _firstName = firstName;
            _loginIdentifier = loginIdentifier;
            _loginProvider = loginProvider;
        }

        public async Task ExecuteAsync(State state, string[] arguments)
        {
            if (!state.ContainsKey(StateKeys.UserId))
            {
                UserLoginModel? userLogin = await _userLoginService.GetUserLoginAsync(_loginProvider, _loginIdentifier);
                if (userLogin == null)
                {
                    UserModel user = await _userService.CreateUserAsync(new UserModel(_firstName));
                    userLogin = await _userLoginService.CreateUserLoginAsync(user.Id, _loginProvider,
                        _loginIdentifier);
                }

                state[StateKeys.UserId] = userLogin.UserId;
            }

            await _botMessageSender.SendAsync(new BotMessage(WelcomeMessage,
                new KeyValuePair<string, string>("Select an account", CommandNames.SelectAccount)));
        }
    }
}
