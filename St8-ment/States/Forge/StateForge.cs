using System;
using System.Collections.Generic;

namespace St8Ment.States.Forge
{
    public class StateForge : IStateForge
    {
        private readonly IDictionary<string, Func<DependencyProvider, IStateForgeCore>> registry;
        private readonly DependencyProvider provider;

        public StateForge(IDictionary<string, Func<DependencyProvider, IStateForgeCore>> registry, DependencyProvider provider)
        {
            this.registry = registry;
            this.provider = provider;
        }

        public IState Connect<TSubject>(TSubject subject) where TSubject : StateSubject
        {
            var core = !registry.TryGetValue(typeof(TSubject).FullName ?? string.Empty, out var coreProvider)
                ? throw new KeyNotFoundException($"Could not find state registration for subject of type '{typeof(TSubject).FullName}'.")
                : coreProvider.Invoke(this.provider);

            return new State<TSubject>(core, subject);
        }
    }
}