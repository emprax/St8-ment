using System;
using System.Collections.Concurrent;

namespace St8_ment.States
{
    public class StateReducerFactory<TKey, TContext> : IStateReducerFactory<TKey, TContext> where TContext : class, IStateContext<TContext>
    {
        private readonly ConcurrentDictionary<TKey, Func<IStateReducerCore<TContext>>> reducers;

        public StateReducerFactory(ConcurrentDictionary<TKey, Func<IStateReducerCore<TContext>>> reducers) => this.reducers = reducers;

        public IStateReducer<TContext> Create(TKey key) => this.reducers.TryGetValue(key, out var core)
            ? new StateReducer<TContext>(core.Invoke())
            : null;
    }
 }
