using System;
using System.Collections.Concurrent;
using System.Linq;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    internal class StateBuilder<TSubject> : IStateBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        private readonly ConcurrentDictionary<string, Func<IServiceProvider, object>> actions;

        internal StateBuilder() => this.actions = new ConcurrentDictionary<string, Func<IServiceProvider, object>>();

        public IActionBuilder<TAction, TSubject> On<TAction>() where TAction : class, IAction
        {
            return new ActionBuilder<TAction, TSubject>(this.actions, this);
        }

        internal Func<IServiceProvider, IActionProvider<TSubject>> Build() 
        { 
            return new Func<IServiceProvider, IActionProvider<TSubject>>(provider =>
            { 
                var core = new ConcurrentDictionary<string, Func<object>>(this.actions
                    .ToDictionary(k => k.Key, k => new Func<object>(() => k.Value.Invoke(provider)))
                    .AsEnumerable());

                return new ActionProvider<TSubject>(core);
            });
        }
    }
}
