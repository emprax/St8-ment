using System;
using System.Collections.Generic;

namespace St8Ment.States.Forge
{
    public class StateForgeCore : IStateForgeCore
    {
        private readonly IDictionary<string, Func<DependencyProvider, IStateCore>> core;
        private readonly DependencyProvider provider;

        public StateForgeCore(IDictionary<string, Func<DependencyProvider, IStateCore>> core, DependencyProvider provider)
        {
            this.core = core;
            this.provider = provider;
        }

        public IStateCore GetForState(StateId id)
        {
            this.core.TryGetValue(id.Name, out var container);
            return container?.Invoke(this.provider);
        }
    }
}