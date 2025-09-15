using St8Ment.StateMachines.Builders;
using System;

namespace St8Ment.StateMachines;

public interface IStateMachineFactory
{
    IStateMachineProvider<TKey> Create<TKey>(Action<IStateMachineFactoryBuilder<TKey>> action) where TKey : notnull;

    IStateMachine Create(Action<IInitialStateComponentBuilder> action);
}
