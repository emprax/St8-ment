using System;

namespace St8Ment.DependencyInjection;

internal class DependencyFactory(IServiceProvider provider) : IDependencyFactory
{
    public object? Create(Type type) => provider.GetService(type);
}