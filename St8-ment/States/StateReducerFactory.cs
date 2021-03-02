using System;
using System.Collections.Concurrent;

namespace St8Ment.States
{
    public class StateReducerFactory<TKey, TSubject> : IStateReducerFactory<TKey, TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        private readonly ConcurrentDictionary<TKey, Func<IStateReducerCore<TSubject>>> reducers;

        public StateReducerFactory(ConcurrentDictionary<TKey, Func<IStateReducerCore<TSubject>>> reducers) => this.reducers = reducers;

        public IStateReducer<TSubject> Create(TKey key) => this.reducers.TryGetValue(key, out var core)
            ? new StateReducer<TSubject>(core.Invoke())
            : null;
    }
 }
