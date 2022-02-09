using System;
using System.Collections.Concurrent;

namespace St8Ment.States
{
    public class StateReducerFactory<TKey, TSubject> : IStateReducerFactory<TKey, TSubject> where TSubject : ExtendedStateSubject<TSubject>
    {
        private readonly ConcurrentDictionary<TKey, Func<DependencyProvider, IStateReducerCore<TSubject>>> reducers;
        private readonly DependencyProvider provider;

        public StateReducerFactory(ConcurrentDictionary<TKey, Func<DependencyProvider, IStateReducerCore<TSubject>>> reducers, DependencyProvider provider)
        {
            this.reducers = reducers;
            this.provider = provider;
        }

        public IStateReducer<TSubject> Create(TKey key) => this.reducers.TryGetValue(key, out var core)
            ? new StateReducer<TSubject>(core.Invoke(this.provider))
            : null;
    }
 }
