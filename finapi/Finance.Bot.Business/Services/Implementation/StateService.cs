using AutoMapper;
using Finance.Bot.Business.Models;
using Finance.Bot.Data.Models;
using Finance.Core.Practices;

namespace Finance.Bot.Business.Services.Implementation
{
    public class StateService : IStateService
    {
        private readonly IRepository<StateEntity, string> _repository;
        private readonly IMapper _mapper;
        private readonly long _chatId;

        public StateService(IRepository<StateEntity, string> stateEntityRepository, IMapper mapper, long chatId)
        {
            _repository = stateEntityRepository ?? throw new ArgumentNullException(nameof(stateEntityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _chatId = chatId;
        }

        public async Task<State> GetStateAsync()
        {
            StateEntity? savedState = await _repository.GetAsync(_chatId.ToString());
            if (savedState != null)
            {
                return _mapper.Map<State>(savedState);
            }

            var newState = new StateEntity { ChatId = _chatId };
            await _repository.InsertAsync(newState);
            return _mapper.Map<State>(newState);
        }

        public async Task UpdateStateAsync(State state)
        {
            await _repository.UpdateAsync(_mapper.Map<StateEntity>(state));
        }
    }
}
