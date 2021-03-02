using System;
using System.Collections.Concurrent;
using System.Linq;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    internal class ActionBuilder<TAction, TSubject> : IActionBuilder<TAction, TSubject>
        where TAction : class, IAction
        where TSubject : class, IStateSubject<TSubject>
    {
        private readonly ConcurrentDictionary<string, Func<IServiceProvider, object>> actions;
        private readonly IStateBuilder<TSubject> builder;

        internal ActionBuilder(ConcurrentDictionary<string, Func<IServiceProvider, object>> actions, IStateBuilder<TSubject> builder)
        {
            this.actions = actions;
            this.builder = builder;
        }

        public IStateBuilder<TSubject> Handle<THandler>() where THandler : class, IActionHandler<TAction, TSubject>
        {
            var key = typeof(TAction).FullName;
            var factory = new Func<IServiceProvider, object>(provider =>
            {
                var constructor = typeof(THandler)?
                    .GetConstructors()?
                    .FirstOrDefault();

                var parameters = constructor?
                    .GetParameters()?
                    .Select(p => provider.GetService(p?.ParameterType))?
                    .ToArray();

                return constructor?.Invoke(parameters) as THandler;
            });

            this.actions.AddOrUpdate(key, k => factory, (k, _) => factory);
            return this.builder;
        }
    }
}
