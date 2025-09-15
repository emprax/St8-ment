using System;

namespace St8Ment.DependencyInjection;

public interface IDependencyFactory
{
    object? Create(Type type);
}