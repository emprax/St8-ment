using St8Ment.States;
using St8Ment.States.Forge;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace St8Ment.DependencyInjection.States.Forge
{
    internal class StateForgeBuilder : IStateForgeBuilder
    {
        private readonly IDictionary<string, Func<DependencyProvider, IStateForgeCore>> core;
        private readonly DependencyProvider provider;

        internal StateForgeBuilder(DependencyProvider provider)
        {
            this.core = new ConcurrentDictionary<string, Func<DependencyProvider, IStateForgeCore>>();
            this.provider = provider;
        }

        internal IStateForge Build() => new StateForge(this.core.ToConcurrentDictionary(), this.provider);

        public IStateForgeBuilder Connect<TSubject>(Action<IStateForgeCoreBuilder<TSubject>> configuration) where TSubject : StateSubject
        {
            var builder = new StateForgeCoreBuilder<TSubject>();
            configuration.Invoke(builder);

            var key = typeof(TSubject).FullName ?? throw new ArgumentNullException("The type of the state subject could not be extracted as key.");
            if (!this.core.ContainsKey(key))
            {
                this.core.Add(key, builder.Build());
                return this;
            }

            this.core[key] = builder.Build();
            return this;
        }
    }
}
