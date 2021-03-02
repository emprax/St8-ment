using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public class StateReducerFactoryBuilder<TKey, TSubject> : IStateReducerFactoryBuilder<TKey, TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        private readonly IDictionary<TKey, Func<IServiceProvider, IStateReducerCore<TSubject>>> stateReducers;

        public StateReducerFactoryBuilder()
        {
            this.stateReducers = new ConcurrentDictionary<TKey, Func<IServiceProvider, IStateReducerCore<TSubject>>>();
        }

        public IStateReducerFactoryBuilder<TKey, TSubject> AddStateReducer(TKey key, Action<IStateReducerBuilder<TSubject>> configuration)
        {
            this.Remove(key);
            this.stateReducers.Add(key, provider =>
            {
                var builder = new StateReducerBuilder<TSubject>();
                configuration.Invoke(builder);
                
                return builder.Build(provider);
            });

            return this;
        }

        internal ConcurrentDictionary<TKey, Func<IStateReducerCore<TSubject>>> Build(IServiceProvider provider)
        {
            return new ConcurrentDictionary<TKey, Func<IStateReducerCore<TSubject>>>(this.stateReducers.ToDictionary(
                k => k.Key,
                k => new Func<IStateReducerCore<TSubject>>(() => k.Value.Invoke(provider))));
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
