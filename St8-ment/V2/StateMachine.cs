using System;
using System.Collections.Generic;

namespace St8_ment.V2
{
    public class StateMachine<TContext> : IStateMachine<TContext> where TContext : class, IStateContext<TContext>
    {
        private readonly IDictionary<int, IStateTransitionerProvider> transitionerProviders;

        public StateMachine(IDictionary<int, IStateTransitionerProvider> transitionerProviders)
        {
            this.transitionerProviders = transitionerProviders ?? throw new ArgumentNullException(nameof(transitionerProviders));
        }

        public IStateTransitionApplier<TState, TContext> For<TState>(TState state) where TState : class, IState<TContext>
        {
            var stateTransitionerProvider = (!this.transitionerProviders.TryGetValue(typeof(TState).GetHashCode(), out var provider) || !(provider is IStateTransitionerProvider<TState, TContext> transitionerProvider))
                ? new EmptyStateTransitionProvider<TState, TContext>()
                : transitionerProvider;

            return new StateTransitionApplier<TState, TContext>(stateTransitionerProvider, state);
        }
    }
}
