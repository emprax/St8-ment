using System;
using System.Collections.Concurrent;
using System.Linq;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    internal class StateBuilder<TSubject> : IStateBuilder<TSubject> where TSubject : ExtendedStateSubject<TSubject>
    {
        private readonly ConcurrentDictionary<string, Func<DependencyProvider, object>> actions;

        internal StateBuilder() => this.actions = new ConcurrentDictionary<string, Func<DependencyProvider, object>>();

        public IActionBuilder<TAction, TSubject> On<TAction>() where TAction : class, IAction => new ActionBuilder<TAction, TSubject>(this.actions);

        internal Func<DependencyProvider, IActionProvider<TSubject>> Build() 
        { 
            return provider =>
            { 
                var core = new ConcurrentDictionary<string, Func<DependencyProvider, object>>(this.actions
                    .ToDictionary(k => k.Key, k => new Func<DependencyProvider, object>(p => k.Value.Invoke(p)))
                    .AsEnumerable());

                return new ActionProvider<TSubject>(core, provider);
            };
        }
    }
}
