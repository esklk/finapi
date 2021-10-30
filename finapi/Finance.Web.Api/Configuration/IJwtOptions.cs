﻿using Microsoft.IdentityModel.Tokens;

namespace Finance.Web.Api.Configuration
{
    public interface IJwtOptions
    {
        string Issuer { get; }

        int LifetimeMinutes { get; }

        string SecurityKeyWord { get; }

        string SecurityAlgorithm { get; }

        SecurityKey SecurityKey { get; }
    }
}
