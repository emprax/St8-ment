﻿using System;
using System.Threading.Tasks;

namespace St8_ment.V2
{
    public static class StateContextExtensions
    {
        public static Task<bool> Apply<TContext, TAction>(this TContext context, IStateMachine<TContext> stateMachine, TAction action) 
            where TAction : IAction
            where TContext : class, IStateContext<TContext>
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.State?
                .Connect(stateMachine)?
                .Apply(action) ?? Task.FromResult(false);
        }
    }
}
