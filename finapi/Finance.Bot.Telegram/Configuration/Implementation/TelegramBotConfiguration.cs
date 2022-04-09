using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Bot.Telegram.Configuration.Implementation
{
    public class TelegramBotConfiguration
    {
        private Uri webhookUrl;

        public string Host { get; set; }

        public string Token { get; set; }

        public Uri WebhookUrl
        {
            get
            {
                if(webhookUrl == null)
                {
                    webhookUrl = new Uri(@$"{Host}/bot/{Token}");
                }

                return webhookUrl;
            }
        }
    }
}
