using St8ment.DependencyInjection.Abstractions;

namespace St8ment.DependencyInjection.Builders;

public class DependencyFactory(IServiceProvider provider) : IDependencyFactory
{
    public object? Create(Type type) => provider.GetService(type);
}