using System;
using System.Collections.Generic;

namespace St8_ment
{
    public class StateTransitionProvider<TState, TContext> : IStateTransitionProvider 
        where TState : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        private readonly IDictionary<int, Func<IStateTransitionMarker>> transitionRegistrations;

        public StateTransitionProvider(IDictionary<int, Func<IStateTransitionMarker>> transitionRegistrations)
        {
            this.transitionRegistrations = transitionRegistrations;
        }

        public IStateTransition<TTransaction> Find<TTransaction>() where TTransaction : ITransaction
        {
            if (!this.transitionRegistrations.TryGetValue(typeof(TTransaction).GetHashCode(), out var value) || !(value.Invoke() is IStateTransition<TTransaction> transition))
            {
                return null;
            }

            return transition;
        }
    }
}