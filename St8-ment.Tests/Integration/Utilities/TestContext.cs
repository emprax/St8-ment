using System.Threading.Tasks;
using St8Ment.States;

namespace St8Ment.Tests.Integration.Utilities
{
    public class TesTSubject : IStateSubject<TesTSubject>
    {
        public IState<TesTSubject> State { get; private set; }

        public TesTSubject() { }

        public TesTSubject(IState<TesTSubject> state) => State = state;

        public void SetState(IState<TesTSubject> state) => State = state;

        public Task<StateResponse> Apply<TAction>(TAction action) where TAction : class, IAction
            => State.Apply(action);
    }
}
