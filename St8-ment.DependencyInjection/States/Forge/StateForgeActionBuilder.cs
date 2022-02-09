using System;
using System.Collections.Generic;
using System.Linq;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States.Forge
{
    internal class StateForgeActionBuilder<TAction, TSubject> : IStateForgeActionBuilder<TAction, TSubject>
        where TAction : class, IAction
        where TSubject : StateSubject
    {
        private readonly IDictionary<string, Func<DependencyProvider, IActionHandler>> core;

        public StateForgeActionBuilder(IDictionary<string, Func<DependencyProvider, IActionHandler>> core) => this.core = core;

        public void Handle<THandler>() where THandler : class, IActionHandler<TAction, TSubject>
        {
            var key = typeof(TAction).FullName;
            var value = new Func<DependencyProvider, IActionHandler>(provider =>
            {
                var constructor = typeof(THandler)
                    .GetConstructors()
                    .FirstOrDefault();

                var parameters = constructor?
                    .GetParameters()
                    .Select(p => provider.Invoke(p.ParameterType))
                    .ToArray() ?? Array.Empty<object>();

                return constructor?.Invoke(parameters) as THandler;
            });

            if (this.core.ContainsKey(key))
            {
                this.core[key] = value;
                return;
            }

            this.core.Add(key, value);
        }
    }
}