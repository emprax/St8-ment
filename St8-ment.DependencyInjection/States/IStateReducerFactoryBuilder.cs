using System;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IStateReducerFactoryBuilder<in TKey, TSubject> where TSubject : ExtendedStateSubject<TSubject>
    {
        IStateReducerFactoryBuilder<TKey, TSubject> AddStateReducer(TKey key, Action<IStateReducerBuilder<TSubject>> configuration);
    }
}
