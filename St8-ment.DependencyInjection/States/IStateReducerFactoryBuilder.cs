using System;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IStateReducerFactoryBuilder<TKey, TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        IStateReducerFactoryBuilder<TKey, TSubject> AddStateReducer(TKey key, Action<IStateReducerBuilder<TSubject>> configuration);
    }
}
