using System;

namespace St8Ment.DependencyInjection.StateMachines.Builders;

public interface IDependencyFactory
{
    object? Get(Type type);
}
