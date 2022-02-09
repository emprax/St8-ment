using System;
using System.Collections.Concurrent;
using System.Linq;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    internal class ActionBuilder<TAction, TSubject> : IActionBuilder<TAction, TSubject>
        where TAction : class, IAction
        where TSubject : ExtendedStateSubject<TSubject>
    {
        private readonly ConcurrentDictionary<string, Func<DependencyProvider, object>> actions;
        
        internal ActionBuilder(ConcurrentDictionary<string, Func<DependencyProvider, object>> actions) => this.actions = actions;

        public void Handle<THandler>() where THandler : class, IActionHandler<TAction, TSubject>
        {
            var key = typeof(TAction).FullName ?? string.Empty;
            var factory = new Func<DependencyProvider, object>(provider =>
            {
                var constructor = typeof(THandler)?
                    .GetConstructors()?
                    .FirstOrDefault();

                var parameters = constructor?
                    .GetParameters()?
                    .Select(p => provider.Invoke(p?.ParameterType))?
                    .ToArray();

                return constructor?.Invoke(parameters) as THandler;
            });

            this.actions.AddOrUpdate(key, k => factory, (k, _) => factory);
        }
    }
}
