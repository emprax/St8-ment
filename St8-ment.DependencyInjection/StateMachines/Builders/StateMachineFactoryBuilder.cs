using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using St8Ment.StateMachines;
using St8Ment.StateMachines.Components;

namespace St8Ment.DependencyInjection.StateMachines.Builders
{
    internal class StateMachineFactoryBuilder<TKey> : IStateMachineFactoryBuilder<TKey>
    {
        private readonly IDictionary<TKey, Func<IServiceProvider, IStateMachineCore>> stateMachines;

        internal StateMachineFactoryBuilder()
        {
            this.stateMachines = new ConcurrentDictionary<TKey, Func<IServiceProvider, IStateMachineCore>>();
        }

        public IStateMachineFactoryBuilder<TKey> AddStateMachine(TKey key, Action<IInitialStateComponentBuilder> configuration)
        {
            this.Remove(key);
            this.stateMachines.Add(key, new Func<IServiceProvider, IStateMachineCore>(provider =>
            { 
                var component = new StateComponentCollection();
                var builder = new InitialStateComponentBuilder(component, provider);
                configuration.Invoke(builder);
                
                return new StateMachineCore(builder.InitialState, component);
            }));

            return this;
        }

        internal ConcurrentDictionary<TKey, Func<IStateMachineCore>> Build(IServiceProvider provider)
        {
            return new ConcurrentDictionary<TKey, Func<IStateMachineCore>>(this.stateMachines.ToDictionary(
                k => k.Key, 
                k => new Func<IStateMachineCore>(() => k.Value.Invoke(provider))));
        }

        private void Remove(TKey key)
        {
            if (this.stateMachines.ContainsKey(key))
            {
                this.stateMachines.Remove(key);
            }
        }
    }
}
