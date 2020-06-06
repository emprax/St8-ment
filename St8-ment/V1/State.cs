using System.Threading;
using System.Threading.Tasks;

namespace St8_ment.V1
{
    public abstract class State<TSelf, TContext> : IState<TContext> 
        where TContext : IStateContext<TContext>
        where TSelf : class, IState<TContext>
    {
        private readonly IStateTransitionerProvider provider;

        protected State(TContext context, IStateTransitionerProvider provider)
        {
            this.Context = context;
            this.provider = provider;
        }

        public TContext Context { get; }

        protected abstract TSelf GetSelf();

        public async Task<bool> Accept<TAction>(TAction action, CancellationToken cancellationToken) where TAction : IAction
        {
            var transitioner = this.provider.Find<StateTransaction<TAction, TSelf>>();
            if (transitioner is null)
            {
                return false;
            }

            await transitioner.Handle(new StateTransaction<TAction, TSelf>(action, this.GetSelf()), cancellationToken);
            return true;
        }
    }
}