using AutoMapper;
using Finance.Web.Api.Extensions;
using Finance.Web.Api.Models;
using System.Collections.Generic;

namespace Finance.Web.Api.Services.Tokens.PayloadMapping.Implementation
{
    public class GoogleAccessTokenConfiguration : MapperConfiguration
    {
        public GoogleAccessTokenConfiguration() : base(Configure) { }

        private static void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<Dictionary<string, string>, LoginPayloadModel>()
                .ForMember(d => d.LoginIdentifier, m => m.MapFromDictionary("Email", true))
                .ForMember(d => d.Name, m => m.MapFromDictionary("Name"));
        }
    }
}
