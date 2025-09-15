using St8Ment.StateMachines.Builders;
using St8Ment.StateMachines.Components;
using System;

namespace St8Ment.StateMachines;

public class StateMachineFactory : IStateMachineFactory
{
    public IStateMachineProvider<TKey> Create<TKey>(Action<IStateMachineFactoryBuilder<TKey>> action) where TKey : notnull
    {
        var builder = new StateMachineFactoryBuilder<TKey>();
        action.Invoke(builder);
        return new StateMachineProvider<TKey>(builder.Build());
    }

    public IStateMachine Create(Action<IInitialStateComponentBuilder> action)
    {
        var component = new StateComponentCollection();
        var builder = new InitialStateComponentBuilder(component);

        action?.Invoke(builder);
        return new StateMachine(new StateMachineCore(builder.InitialState, component));
    }
}
