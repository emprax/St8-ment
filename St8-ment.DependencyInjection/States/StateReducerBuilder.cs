﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    internal class StateReducerBuilder<TSubject> : IStateReducerBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        private readonly ConcurrentDictionary<StateId, Func<IServiceProvider, IActionProvider<TSubject>>> states;

        internal StateReducerBuilder() => this.states = new ConcurrentDictionary<StateId, Func<IServiceProvider, IActionProvider<TSubject>>>();

        public IStateReducerBuilder<TSubject> For(StateId stateId)
        {
            this.states.GetOrAdd(stateId, _ => new ActionProvider<TSubject>(new ConcurrentDictionary<string, Func<object>>()));
            return this;
        }

        public IStateReducerBuilder<TSubject> For(StateId stateId, Action<IStateBuilder<TSubject>> configuration)
        {
            var builder = new StateBuilder<TSubject>();
            configuration.Invoke(builder);
            
            var factory = builder.Build();
            this.states.AddOrUpdate(stateId, key => factory, (key, _) => factory);

            return this;
        }

        internal IStateReducerCore<TSubject> Build(IServiceProvider provider)
        {
            var core = new ConcurrentDictionary<StateId, IActionProvider<TSubject>>(this.states.ToDictionary(k => k.Key, k => k.Value?.Invoke(provider)));
            return new StateReducerCore<TSubject>(core);
        }
    }
}
