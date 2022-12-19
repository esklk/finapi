using Finance.Bot.Business.Services.Implementation.Stateful;
using Finance.Business.Services;
using System;
using Telegram.Bot.Types;

namespace Finance.Bot.Telegram.Services.Implementation
{
    public class TelegramStartedMessageProcessor : StartedMessageProcessor
    {
        private readonly User _user;

        public TelegramStartedMessageProcessor(IUpdateProvider updateProvider, IUserService userService, IUserLoginService userLoginService) : base(userService, userLoginService)
        {
            if(updateProvider == null)
            {
                throw new ArgumentNullException(nameof(updateProvider));
            }

            _user = updateProvider.Update.GetUser();
        }

        protected override string UserFirstName => _user.FirstName;

        protected override string LoginIdentifier => _user.Id.ToString();

        protected override string LoginProvider => "Telegram";
    }
}
