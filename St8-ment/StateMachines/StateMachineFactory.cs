using System;
using System.Collections.Generic;

namespace St8Ment.StateMachines
{
    public class StateMachineFactory<TKey> : IStateMachineFactory<TKey>
    {
        private readonly IDictionary<TKey, Func<IStateMachineCore>> stateMachines;

        public StateMachineFactory(IDictionary<TKey, Func<IStateMachineCore>> stateMachines) => this.stateMachines = stateMachines;

        public IStateMachine Create(TKey key) => this.stateMachines.TryGetValue(key, out var factory)
            ? new StateMachine(factory.Invoke())
            : null;
    }
}
