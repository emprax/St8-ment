using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using St8Ment.States;
using St8Ment.States.Forge;

namespace St8Ment.DependencyInjection.States.Forge
{
    internal class StateForgeStateBuilder<TSubject> : IStateForgeStateBuilder<TSubject> where TSubject : StateSubject
    {
        private readonly IDictionary<string, Func<DependencyProvider, IActionHandler>> core;

        public StateForgeStateBuilder() => this.core = new ConcurrentDictionary<string, Func<DependencyProvider, IActionHandler>>();

        internal Func<DependencyProvider, IStateCore> Build()
        {
            var registry = this.core.ToConcurrentDictionary();
            return provider => new StateCore(registry, provider);
        }

        public IStateForgeActionBuilder<TAction, TSubject> On<TAction>() where TAction : class, IAction => new StateForgeActionBuilder<TAction, TSubject>(this.core);
    }
}