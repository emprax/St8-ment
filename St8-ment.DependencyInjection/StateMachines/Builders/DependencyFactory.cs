using System;

namespace St8Ment.DependencyInjection.StateMachines.Builders;

public class DependencyFactory(IServiceProvider provider) : IDependencyFactory
{
    public object? Get(Type type) => provider.GetService(type);
}
