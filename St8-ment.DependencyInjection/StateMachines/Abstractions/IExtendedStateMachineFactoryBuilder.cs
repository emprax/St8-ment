using System;

namespace St8Ment.DependencyInjection.StateMachines.Abstractions;

public interface IExtendedStateMachineFactoryBuilder<TKey> where TKey : notnull
{
    IExtendedStateMachineFactoryBuilder<TKey> AddStateMachine(TKey key, Action<IExtendedInitialStateComponentBuilder> configuration);
}
