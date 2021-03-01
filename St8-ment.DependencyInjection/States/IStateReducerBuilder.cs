using System;
using St8_ment.States;

namespace St8_ment.DependencyInjection.States
{
    public interface IStateReducerBuilder<TContext> where TContext : class, IStateContext<TContext>
    {
        IStateReducerBuilder<TContext> For(StateId stateId);

        IStateReducerBuilder<TContext> For(StateId stateId, Action<IStateBuilder<TContext>> configuration);
    }
}
