using AutoMapper;
using Finance.Bot.Data.Models;
using Finance.Core.Practices;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Bot.Business.Services.Implementation
{
    public class StateServiceFactory: IFactory<IStateService, string>
    {
        private readonly IServiceProvider _serviceProvider;

        public StateServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IStateService Create(string stateId)
        {
            return new StateService(
                _serviceProvider.GetRequiredService<IRepository<StateEntity, string>>(),
                _serviceProvider.GetRequiredService<IMapper>(),
                stateId);
        }
    }
}
