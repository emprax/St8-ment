using System;

namespace St8Ment.DependencyInjection.StateMachines.Builders;

public interface IExtendedStateMachineFactoryBuilder<TKey> where TKey : notnull
{
    IExtendedStateMachineFactoryBuilder<TKey> AddStateMachine(TKey key, Action<IExtendedInitialStateComponentBuilder> configuration);
}
