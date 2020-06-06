using System;
using System.Collections.Generic;

namespace St8_ment.V2
{
    public class StateTransitionerProvider<TState, TContext> : IStateTransitionerProvider<TState, TContext>
        where TState : IState<TContext>
        where TContext : IStateContext<TContext>
    {
        private readonly IDictionary<int, Func<IStateTransitionerMarker>> transitioners;

        public StateTransitionerProvider(IDictionary<int, Func<IStateTransitionerMarker>> transitioners)
        {
            this.transitioners = transitioners;
        }

        public IStateTransitioner<TAction, TState, TContext> Find<TAction>() where TAction : IAction
        {
            transitioners.TryGetValue(typeof(IStateTransaction<TAction, TState>).GetHashCode(), out var transitioner);
            return transitioner?.Invoke() as IStateTransitioner<TAction, TState, TContext>;
        }
    }
}