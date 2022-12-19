using AutoMapper;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Bot.Business.Services.Implementation;
using Finance.Bot.Data.Models;
using Finance.Core.Practices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Bot.Telegram.Services.Implementation
{
    public class TelegramChatStateServiceFactory : IFactory<IStateService>
    {
        private readonly long _chatId;
        private readonly IFactory<IStateService, string> _stateServiceFactory;

        public TelegramChatStateServiceFactory(IUpdateProvider updateProvider, IFactory<IStateService, string> stateServiceFactory)
        {
            _chatId = updateProvider.Update.GetChat().Id;
            _stateServiceFactory = stateServiceFactory ?? throw new ArgumentNullException(nameof(stateServiceFactory));
        }

        public IStateService Create()
        {
            return _stateServiceFactory.Create(_chatId.ToString());
        }
    }
}
