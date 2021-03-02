using System;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IStateReducerFactoryBuilder<TKey, TContext> where TContext : class, IStateContext<TContext>
    {
        IStateReducerFactoryBuilder<TKey, TContext> AddStateReducer(TKey key, Action<IStateReducerBuilder<TContext>> configuration);
    }
}
