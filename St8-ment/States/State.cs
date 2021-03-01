using System.Threading.Tasks;

namespace St8_ment.States
{
    public class State<TContext> : IState<TContext> where TContext : class, IStateContext<TContext>
    {
        private readonly IStateReducer<TContext> reducer;

        public State(StateId id, TContext context, IStateReducer<TContext> reducer)
        {
            this.StateId = id;
            this.Context = context;
            this.reducer = reducer;
        }

        public TContext Context { get; }

        public StateId StateId { get; }

        public async Task<StateResponse> Apply<TAction>(TAction action) where TAction : class, IAction
        {
            if (!this.reducer.TryGetProvider(this.StateId, out var provider))
            {
                return StateResponse.ToStateActionsNotFound(this.StateId);
            }

            var actionName = typeof(TAction).Name;
            if (provider.TryGet<TAction>(out var handler))
            {
                var stateId = await handler.Execute(action, this);
                if (stateId.Name != this.StateId.Name)
                {
                    this.reducer.SetState(stateId, this.Context);
                }

                return StateResponse.ToSuccess(this.StateId, stateId, actionName);
            }

            return StateResponse.ToNoMatchingAction(this.StateId, actionName);
        }
    }
}
