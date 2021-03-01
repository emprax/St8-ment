using System;
using System.Collections.Concurrent;
using System.Linq;
using St8_ment.States;

namespace St8_ment.DependencyInjection.States
{
    internal class StateReducerBuilder<TContext> : IStateReducerBuilder<TContext> where TContext : class, IStateContext<TContext>
    {
        private readonly ConcurrentDictionary<StateId, Func<IServiceProvider, IActionProvider<TContext>>> states;

        internal StateReducerBuilder() => this.states = new ConcurrentDictionary<StateId, Func<IServiceProvider, IActionProvider<TContext>>>();

        public IStateReducerBuilder<TContext> For(StateId stateId)
        {
            this.states.GetOrAdd(stateId, _ => new ActionProvider<TContext>(new ConcurrentDictionary<string, Func<object>>()));
            return this;
        }

        public IStateReducerBuilder<TContext> For(StateId stateId, Action<IStateBuilder<TContext>> configuration)
        {
            var builder = new StateBuilder<TContext>();
            configuration.Invoke(builder);
            
            var factory = builder.Build();
            this.states.AddOrUpdate(stateId, key => factory, (key, _) => factory);

            return this;
        }

        internal IStateReducerCore<TContext> Build(IServiceProvider provider)
        {
            var core = new ConcurrentDictionary<StateId, IActionProvider<TContext>>(this.states.ToDictionary(k => k.Key, k => k.Value?.Invoke(provider)));
            return new StateReducerCore<TContext>(core);
        }
    }
}
