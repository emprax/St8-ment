using System;
using System.Collections.Concurrent;

namespace St8Ment.States
{
    public class ActionProvider<TContext> : IActionProvider<TContext> where TContext : class, IStateContext<TContext>
    {
        private readonly ConcurrentDictionary<string, Func<object>> handlers;

        public ActionProvider(ConcurrentDictionary<string, Func<object>> handlers)
        {
            this.handlers = handlers;
        }

        public bool TryGet<TAction>(out IActionHandler<TAction, TContext> actionHandler) where TAction : class, IAction
        {
            if (this.handlers.TryGetValue(typeof(TAction).FullName, out var factory) && factory?.Invoke() is IActionHandler<TAction, TContext> handler)
            {
                actionHandler = handler;
                return true;
            }

            actionHandler = null;
            return false;
        }
    }
}
