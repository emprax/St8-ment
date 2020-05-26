using System;
using System.Collections.Generic;

namespace St8_ment
{
    public class StateMachine<TContext> : IStateMachine<TContext> where TContext : IStateContext
    {
        private readonly IDictionary<int, IState<TContext>> registrations;

        public StateMachine(IDictionary<int, IState<TContext>> registrations)
        {
            this.registrations = registrations;
        }

        public TState Find<TState>() where TState : class, IState<TContext>
        {
            if (!this.registrations.TryGetValue(typeof(TState).GetHashCode(), out var value) || !(value is TState state))
            {
                return null;
            }

            return state;
        }
    }
}