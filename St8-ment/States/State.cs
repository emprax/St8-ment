using System.Threading.Tasks;

namespace St8Ment.States
{
    public class State<TSubject> : IState<TSubject> where TSubject : ExtendedStateSubject<TSubject>
    {
        private readonly IStateReducer<TSubject> reducer;

        public State(StateId id, TSubject subject, IStateReducer<TSubject> reducer)
        {
            this.Id = id;
            this.Subject = subject;
            this.reducer = reducer;
        }

        public TSubject Subject { get; }

        public void Transition(StateId nextState) => this.reducer.SetState(nextState, this.Subject);

        public StateId Id { get; }

        public async Task<StateResponse> Apply<TAction>(TAction action) where TAction : class, IAction
        {
            if (!this.reducer.TryGetProvider(this.Id, out var provider))
            {
                return StateResponse.ToStateActionsNotFound(this.Id);
            }

            var actionName = typeof(TAction).Name;
            if (!provider.TryGet<TAction>(out var handler))
            {
                return StateResponse.ToNoMatchingAction(this.Id, actionName);
            }

            await handler.Execute(action, this);
            return StateResponse.ToSuccess(this.Id, actionName);
        }
    }
}
