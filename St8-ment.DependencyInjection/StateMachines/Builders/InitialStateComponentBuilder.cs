using System;
using St8Ment.StateMachines.Components;

namespace St8Ment.DependencyInjection.StateMachines.Builders
{
    internal class InitialStateComponentBuilder : IInitialStateComponentBuilder
    {
        private readonly IKeyValueStateComponent<StateId> parent;
        private readonly IServiceProvider provider;

        public InitialStateComponentBuilder(IKeyValueStateComponent<StateId> parent, IServiceProvider provider)
        {
            this.parent = parent;
            this.provider = provider;
        }

        internal StateId InitialState { get; private set; }

        public IStateComponentCollectionBuilder ForInitial(StateId stateId, Action<IStateComponentBuilder> configuration)
        {
            var component = this.GetOrAdd(stateId);
            configuration.Invoke(new StateComponentBuilder(component, this.provider));
            this.InitialState = stateId;

            return new StateComponentCollectionBuilder(this.parent, this.provider);
        }

        public IStateComponentCollectionBuilder ForInitial(IStateConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new InvalidOperationException("When using state-configurations these should exist for the initial state.");
            }

            var component = this.GetOrAdd(configuration.StateId);
            configuration.Configure(new StateComponentBuilder(component, this.provider));
            this.InitialState = configuration.StateId;

            return new StateComponentCollectionBuilder(this.parent, this.provider);
        }

        private StateComponent GetOrAdd(StateId id)
        {
            if (!this.parent.TryGetValue(id, out var component))
            {
                component = new StateComponent();
                this.parent.Add(id, component);
            }

            return component as StateComponent;
        }
    }
}
