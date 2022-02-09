using System;
using System.Collections.Concurrent;

namespace St8Ment.States
{
    public class ActionProvider<TSubject> : IActionProvider<TSubject> where TSubject : StateSubject
    {
        private readonly ConcurrentDictionary<string, Func<DependencyProvider, object>> handlers;
        private readonly DependencyProvider provider;

        public ActionProvider(ConcurrentDictionary<string, Func<DependencyProvider, object>> handlers, DependencyProvider provider)
        {
            this.handlers = handlers;
            this.provider = provider;
        }

        public bool TryGet<TAction>(out IActionHandler<TAction, TSubject> actionHandler) where TAction : class, IAction
        {
            var key = typeof(TAction).FullName ?? string.Empty;
            if (this.handlers.TryGetValue(key, out var factory) && factory?.Invoke(this.provider) is IActionHandler<TAction, TSubject> handler)
            {
                actionHandler = handler;
                return true;
            }

            actionHandler = null;
            return false;
        }
    }
}
