using System;

namespace Finance.Core.Practices
{
    public class ServiceProviderFactory<T> : IFactory<T, Type>
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public T Create(Type type)
        {
            if(type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsAssignableTo(typeof(T)))
            {
                throw new ArgumentException($"Provided type \"{type.AssemblyQualifiedName}\" is not assignable to \"{typeof(T).AssemblyQualifiedName}\".");
            }

            return (T)(_serviceProvider.GetService(type) ?? throw new InvalidOperationException($"No service registered for type \"{type.AssemblyQualifiedName}\"."));
        }
    }
}
