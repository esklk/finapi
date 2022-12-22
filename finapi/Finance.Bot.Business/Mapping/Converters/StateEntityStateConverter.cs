using AutoMapper;
using Finance.Bot.Business.Models;
using Finance.Bot.Data.Models;
using Newtonsoft.Json;

namespace Finance.Bot.Business.Mapping.Converters
{
    public class StateEntityStateConverter : ITypeConverter<StateEntity, State>, ITypeConverter<State, StateEntity>
    {
        public State Convert(StateEntity source, State destination, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(source.DataDictionary)
                ? new State()
                : new State(JsonConvert.DeserializeObject<Dictionary<string, string>>(source.DataDictionary));
        }

        public StateEntity Convert(State source, StateEntity destination, ResolutionContext context)
        {
            return new StateEntity
            {
                DataDictionary = JsonConvert.SerializeObject(source.Data)
            };
        }
    }
}
