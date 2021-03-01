using System;
using System.Collections.Concurrent;
using System.Linq;
using St8_ment.States;

namespace St8_ment.DependencyInjection.States
{
    internal class ActionBuilder<TAction, TContext> : IActionBuilder<TAction, TContext>
        where TAction : class, IAction
        where TContext : class, IStateContext<TContext>
    {
        private readonly ConcurrentDictionary<string, Func<IServiceProvider, object>> actions;
        private readonly IStateBuilder<TContext> builder;

        internal ActionBuilder(ConcurrentDictionary<string, Func<IServiceProvider, object>> actions, IStateBuilder<TContext> builder)
        {
            this.actions = actions;
            this.builder = builder;
        }

        public IStateBuilder<TContext> Handle<THandler>() where THandler : class, IActionHandler<TAction, TContext>
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
