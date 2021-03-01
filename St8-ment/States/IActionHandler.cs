﻿using System.Threading.Tasks;

namespace St8_ment.States
{
    public interface IActionHandler<TAction, TContext>
        where TContext : class, IStateContext<TContext>
        where TAction : class, IAction
    {
        Task<StateId> Execute(TAction action, IStateView<TContext> state);
    }
}
