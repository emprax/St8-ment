using System;
using System.Collections.Concurrent;
using System.Linq;
using St8_ment.States;

namespace St8_ment.DependencyInjection.States
{
    internal class StateBuilder<TContext> : IStateBuilder<TContext> where TContext : class, IStateContext<TContext>
    {
        private readonly ConcurrentDictionary<string, Func<IServiceProvider, object>> actions;

        internal StateBuilder() => this.actions = new ConcurrentDictionary<string, Func<IServiceProvider, object>>();

        public IActionBuilder<TAction, TContext> On<TAction>() where TAction : class, IAction
        {
            return new ActionBuilder<TAction, TContext>(this.actions, this);
        }

        internal Func<IServiceProvider, IActionProvider<TContext>> Build() 
        { 
            return new Func<IServiceProvider, IActionProvider<TContext>>(provider =>
            { 
                var core = new ConcurrentDictionary<string, Func<object>>(this.actions
                    .ToDictionary(k => k.Key, k => new Func<object>(() => k.Value.Invoke(provider)))
                    .AsEnumerable());

                return new ActionProvider<TContext>(core);
            });
        }
    }
}
