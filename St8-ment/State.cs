using System.Threading;
using System.Threading.Tasks;

namespace St8_ment
{
    public abstract class State<TSelf, TContext> : IState<TContext> 
        where TContext : IStateContext<TContext>
        where TSelf : class, IState<TContext>
    {
        private readonly IStateTransitionProvider provider;

        protected State(TContext context, IStateTransitionProvider provider)
        {
            this.Context = context;
            this.provider = provider;
        }

        public TContext Context { get; }

        protected abstract TSelf GetSelf();

        public async Task<bool> Accept<TAction>(TAction action, CancellationToken cancellationToken) where TAction : IAction
        {
            var transition = this.provider.Find<StateTransaction<TAction, TSelf>>();
            if (transition is null)
            {
                return false;
            }

            await transition.Handle(new StateTransaction<TAction, TSelf>(action, this.GetSelf()), cancellationToken);
            return true;
        }
    }
}