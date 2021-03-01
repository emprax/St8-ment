using System;

namespace St8_ment.DependencyInjection.StateMachines.Builders
{
    public interface IStateMachineFactoryBuilder<TKey>
    {
        IStateMachineFactoryBuilder<TKey> AddStateMachine(TKey key, Action<IInitialStateComponentBuilder> configuration);
    }
}
