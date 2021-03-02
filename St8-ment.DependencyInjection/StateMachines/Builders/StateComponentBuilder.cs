using System;
using St8Ment.StateMachines.Components;

namespace St8Ment.DependencyInjection.StateMachines.Builders
{
    internal class StateComponentBuilder : IStateComponentBuilder
    {
        private readonly IKeyValueStateComponent<string> parent;
        private readonly IServiceProvider provider;

        public StateComponentBuilder(IKeyValueStateComponent<string> parent, IServiceProvider provider)
        {
            this.parent = parent;
            this.provider = provider;
        }

        public IStateTransitionBuilder<TInput> On<TInput>()
        {
            var component = GetOrAdd<TInput>();
            return new StateTransitionBuilder<TInput>(this, component, provider);
        }

        public IStateTransitionBuilder<object> OnDefault()
        {
            var component = GetOrAdd<object>();
            return new StateTransitionBuilder<object>(this, component, provider);
        }

        private IItemStateComponent GetOrAdd<TInput>()
        {
            var key = typeof(TInput).FullName;
            if (!this.parent.TryGetValue(key, out var component))
            {
                component = new ActionsComponent();
                this.parent.Add(key, component);
            }

            return component as IItemStateComponent;
        }
    }
}
