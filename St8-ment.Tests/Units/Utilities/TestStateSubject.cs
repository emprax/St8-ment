using System.Threading.Tasks;
using St8Ment.States;

namespace St8Ment.Tests.Units.Utilities
{
    public class TestStateSubject : IStateSubject<TestStateSubject>
    {
        public IState<TestStateSubject> State { get; private set; }

        public TestStateSubject() { }

        public TestStateSubject(IState<TestStateSubject> state) => this.State = state;

        public void SetState(IState<TestStateSubject> state) => this.State = state;

        public Task<StateResponse> Apply<TAction>(TAction action) where TAction : class, IAction
            => this.State.Apply(action);
    }
}
