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
        private readonly string _stateId;

        public StateService(IRepository<StateEntity, string> stateEntityRepository, IMapper mapper, string stateId)
        {
            if (string.IsNullOrWhiteSpace(stateId))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(stateId));
            }
            _repository = stateEntityRepository ?? throw new ArgumentNullException(nameof(stateEntityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _stateId = stateId;
        }

        public async Task<State?> GetStateAsync()
        {
            StateEntity? savedState = await _repository.GetAsync(_stateId);
            return _mapper.Map<State>(savedState);
        }

        public async Task SetStateAsync(State state)
        {
            var stateEntity = _mapper.Map<StateEntity>(state);
            stateEntity.RowKey = _stateId;
            
            await _repository.UpdateAsync(stateEntity);
        }
    }
}
