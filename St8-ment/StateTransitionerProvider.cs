using System;
using System.Collections.Generic;

namespace St8_ment
{
    public class StateTransitionerProvider<TState, TContext> : IStateTransitionerProvider 
        where TState : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        private readonly IDictionary<int, Func<IStateTransitionerMarker>> transitionerRegistrations;

        public StateTransitionerProvider(IDictionary<int, Func<IStateTransitionerMarker>> transitionRegistrations)
        {
            this.transitionerRegistrations = transitionRegistrations;
        }

        public IStateTransitioner<TTransaction> Find<TTransaction>() where TTransaction : ITransaction
        {
            if (!this.transitionerRegistrations.TryGetValue(typeof(TTransaction).GetHashCode(), out var value) || !(value.Invoke() is IStateTransitioner<TTransaction> transition))
            {
                return null;
            }

            return transition;
        }
    }
}