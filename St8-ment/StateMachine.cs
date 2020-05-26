﻿using System;
using System.Collections.Generic;

namespace St8_ment
{
    public class StateMachine<TContext> : IStateMachine<TContext> where TContext : IStateContext<TContext>
    {
        private readonly IDictionary<int, Func<TContext, IState<TContext>>> registrations;

        public StateMachine(IDictionary<int, Func<TContext, IState<TContext>>> registrations)
        {
            this.registrations = registrations;
        }

        public TState Find<TState>(TContext context) where TState : class, IState<TContext>
        {
            if (!this.registrations.TryGetValue(typeof(TState).GetHashCode(), out var value) || !(value?.Invoke(context) is TState state))
            {
                return null;
            }

            return state;
        }
    }
}