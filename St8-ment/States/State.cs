using System.Threading.Tasks;

namespace St8Ment.States
{
    public class State<TSubject> : IState<TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        private readonly IStateReducer<TSubject> reducer;

        public State(StateId id, TSubject subject, IStateReducer<TSubject> reducer)
        {
            this.StateId = id;
            this.Subject = subject;
            this.reducer = reducer;
        }

        public TSubject Subject { get; }

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
                    this.reducer.SetState(stateId, this.Subject);
                }

                return StateResponse.ToSuccess(this.StateId, stateId, actionName);
            }

            return StateResponse.ToNoMatchingAction(this.StateId, actionName);
        }
    }
}
