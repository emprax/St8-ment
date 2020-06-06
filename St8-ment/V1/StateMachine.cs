using System;
using System.Collections.Generic;

namespace St8_ment.V1
{
    public class StateMachine<TContext> : IStateMachine<TContext> where TContext : IStateContext<TContext>
    {
        private readonly IDictionary<int, Func<TContext, IState<TContext>>> registrations;

        public StateMachine(IDictionary<int, Func<TContext, IState<TContext>>> registrations)
        {
            this.registrations = registrations;
        }

        public bool Apply<TState>(TContext context) where TState : class, IState<TContext>
        {
            if (!this.registrations.TryGetValue(typeof(TState).GetHashCode(), out var value) || !(value?.Invoke(context) is TState state))
            {
                return false;
            }

            context.SetState(state);
            return true;
        }
    }
}