using St8Ment.States;

namespace St8Ment.Tests.Units.Utilities
{
    public class TestExtendedStateSubject : ExtendedStateSubject<TestExtendedStateSubject>
    {
        public TestExtendedStateSubject() { }

        public TestExtendedStateSubject(StateId stateId) => this.StateId = stateId;

        public TestExtendedStateSubject(IState<TestExtendedStateSubject> state) => this.SetState(state);
    }
}
