using System.Threading.Tasks;
using St8Ment.States;

namespace St8Ment.Tests.Integration.Utilities
{
    public class TesTSubject : ExtendedStateSubject<TesTSubject>
    {
        public TesTSubject() { }

        public TesTSubject(StateId id) => this.StateId = id;

        public TesTSubject(IState<TesTSubject> state) => this.SetState(state);
    }
}
