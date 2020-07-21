using System;
using System.Threading.Tasks;

namespace St8_ment.V2
{
    public class StateTransitionApplier<TState, TContext> : IStateTransitionApplier<TState, TContext>
        where TState : class, IState<TContext>
        where TContext : class, IStateContext<TContext>
    {
        private readonly IStateTransitionerProvider<TState, TContext> provider;
        private readonly TState state;

        public StateTransitionApplier(IStateTransitionerProvider<TState, TContext> provider, TState state)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.state = state;
        }

        public Task<bool> Apply<TAction>(TAction action) where TAction : IAction
        {
            return provider.Find<TAction>()?.Apply(new StateTransaction<TAction, TState>(action, this.state)) ?? Task.FromResult(false);
        }
    }
}
