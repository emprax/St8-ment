using System;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IStateReducerBuilder<TContext> where TContext : class, IStateContext<TContext>
    {
        IStateReducerBuilder<TContext> For(StateId stateId);

        IStateReducerBuilder<TContext> For(StateId stateId, Action<IStateBuilder<TContext>> configuration);
    }
}
