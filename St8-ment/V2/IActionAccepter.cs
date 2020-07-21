﻿using System.Threading.Tasks;

namespace St8_ment.V2
{
    public interface IActionAccepter<TContext> where TContext : class, IStateContext<TContext>
    {
        Task<bool> Apply<TAction>(TAction action) where TAction : IAction;
    }
}
