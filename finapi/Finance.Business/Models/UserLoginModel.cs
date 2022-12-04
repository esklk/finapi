using System;

namespace Finance.Business.Models
{
    public class UserLoginModel
    {
        public UserLoginModel(string identifier, string provider, int userId)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(identifier));
            }

            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(provider));
            }
            Identifier = identifier;
            Provider = provider;
            UserId = userId;
        }

        public string Identifier { get; set; }

        public string Provider { get; set; }

        public int UserId { get; set; }
    }
}
