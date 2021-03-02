using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public class StateReducerFactoryBuilder<TKey, TContext> : IStateReducerFactoryBuilder<TKey, TContext> where TContext : class, IStateContext<TContext>
    {
        private readonly IDictionary<TKey, Func<IServiceProvider, IStateReducerCore<TContext>>> stateReducers;

        public StateReducerFactoryBuilder()
        {
            this.stateReducers = new ConcurrentDictionary<TKey, Func<IServiceProvider, IStateReducerCore<TContext>>>();
        }

        public IStateReducerFactoryBuilder<TKey, TContext> AddStateReducer(TKey key, Action<IStateReducerBuilder<TContext>> configuration)
        {
            this.Remove(key);
            this.stateReducers.Add(key, provider =>
            {
                var builder = new StateReducerBuilder<TContext>();
                configuration.Invoke(builder);
                
                return builder.Build(provider);
            });

            return this;
        }

        internal ConcurrentDictionary<TKey, Func<IStateReducerCore<TContext>>> Build(IServiceProvider provider)
        {
            return new ConcurrentDictionary<TKey, Func<IStateReducerCore<TContext>>>(this.stateReducers.ToDictionary(
                k => k.Key,
                k => new Func<IStateReducerCore<TContext>>(() => k.Value.Invoke(provider))));
        }

        private void Remove(TKey key)
        {
            if (this.stateReducers.ContainsKey(key))
            {
                this.stateReducers.Remove(key);
            }
        }
    }
}
