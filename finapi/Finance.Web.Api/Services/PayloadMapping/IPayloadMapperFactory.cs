using AutoMapper;

namespace Finance.Web.Api.Services.Tokens.PayloadMapping
{
    public interface IPayloadMapperFactory
    {
        IMapper Create(string provider);
    }
}
