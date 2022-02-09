using St8Ment.States;
using St8Ment.States.Forge;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace St8Ment.DependencyInjection.States.Forge
{
    internal class StateForgeCoreBuilder<TSubject> : IStateForgeCoreBuilder<TSubject> where TSubject : StateSubject
    {
        private readonly IDictionary<string, Func<DependencyProvider, IStateCore>> core;
        
        internal StateForgeCoreBuilder() => this.core = new ConcurrentDictionary<string, Func<DependencyProvider, IStateCore>>();

        internal Func<DependencyProvider, IStateForgeCore> Build()
        {
            var registry = this.core.ToConcurrentDictionary();
            return provider => new StateForgeCore(registry, provider);
        }

        public IStateForgeCoreBuilder<TSubject> For(StateId stateId)
        {
            if (!this.core.ContainsKey(stateId.Name))
            {
                this.core.Add(stateId.Name, _ => null);
                return this;
            }

            this.core[stateId.Name] = _ => null;
            return this;
        }

        public IStateForgeCoreBuilder<TSubject> For(IStateForgeStateConfiguration<TSubject> configuration)
        {
            if (configuration is null)
            {
                return this;
            }

            var builder = new StateForgeStateBuilder<TSubject>();
            configuration.Configure(builder);

            var factory = builder.Build();
            if (!this.core.ContainsKey(configuration.StateId.Name))
            {
                this.core.Add(configuration.StateId.Name, factory);
                return this;
            }

            this.core[configuration.StateId.Name] = factory;
            return this;
        }

        public IStateForgeCoreBuilder<TSubject> For(StateId stateId, Action<IStateForgeStateBuilder<TSubject>> configuration)
        {
            var builder = new StateForgeStateBuilder<TSubject>();
            configuration.Invoke(builder);

            if (!this.core.ContainsKey(stateId.Name))
            {
                this.core.Add(stateId.Name, builder.Build());
                return this;
            }

            this.core[stateId.Name] = builder.Build();
            return this;
        }
    }
}