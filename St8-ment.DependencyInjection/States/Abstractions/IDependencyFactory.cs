namespace St8ment.DependencyInjection.Abstractions;

public interface IDependencyFactory
{
    object? Create(Type type);
}