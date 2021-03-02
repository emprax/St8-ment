using System;
using St8Ment.StateMachines.Components;

namespace St8Ment.DependencyInjection.StateMachines.Builders
{
    internal class StateComponentCollectionBuilder : IStateComponentCollectionBuilder
    {
        private readonly IKeyValueStateComponent<StateId> parent;
        private readonly IServiceProvider provider;

        public StateComponentCollectionBuilder(IKeyValueStateComponent<StateId> parent, IServiceProvider provider)
        {
            this.parent = parent;
            this.provider = provider;
        }

        public IStateComponentCollectionBuilder For(StateId stateId, Action<IStateComponentBuilder> configuration)
        {
            var component = this.GetOrAdd(stateId);
            configuration.Invoke(new StateComponentBuilder(component, this.provider));

            return this;
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
