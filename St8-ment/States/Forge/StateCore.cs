using System;
using System.Collections.Generic;

namespace St8Ment.States.Forge
{
    public class StateCore : IStateCore
    {
        private readonly IDictionary<string, Func<DependencyProvider, IActionHandler>> handlers;
        private readonly DependencyProvider provider;

        public StateCore(IDictionary<string, Func<DependencyProvider, IActionHandler>> handlers, DependencyProvider provider)
        {
            this.handlers = handlers;
            this.provider = provider;
        }

        public IActionHandler GetHandler<TAction>() where TAction : class, IAction
        {
            this.handlers.TryGetValue(typeof(TAction).FullName ?? string.Empty, out var handlerProvider);
            return handlerProvider?.Invoke(this.provider);
        }
    }
}