using System;
using St8_ment.StateMachines.Components;

namespace St8_ment.DependencyInjection.StateMachines.Builders
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
