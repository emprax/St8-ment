using System;
using St8_ment.States;

namespace St8_ment.DependencyInjection.States
{
    public interface IStateReducerFactoryBuilder<TKey, TContext> where TContext : class, IStateContext<TContext>
    {
        IStateReducerFactoryBuilder<TKey, TContext> AddStateReducer(TKey key, Action<IStateReducerBuilder<TContext>> configuration);
    }
}
