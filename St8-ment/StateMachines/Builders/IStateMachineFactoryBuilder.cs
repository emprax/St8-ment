using System;

namespace St8Ment.StateMachines.Builders;

public interface IStateMachineFactoryBuilder<TKey>
{
    IStateMachineFactoryBuilder<TKey> AddStateMachine(TKey key, Action<IInitialStateComponentBuilder> configuration);
}