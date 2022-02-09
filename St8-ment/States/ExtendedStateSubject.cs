using System.Threading.Tasks;

namespace St8Ment.States
{
    public abstract class ExtendedStateSubject<TSubject> : StateSubject where TSubject : ExtendedStateSubject<TSubject>
    {
        private IState<TSubject> state;

        protected ExtendedStateSubject() { }

        protected ExtendedStateSubject(IState<TSubject> state) => this.state = state;

        public virtual Task<StateResponse> Apply<TAction>(TAction action) where TAction : class, IAction => this.state.Apply(action);

        protected internal void SetState(IState<TSubject> state)
        {
            this.state = state;
            this.StateId = state.Id;
        }
    }
}
