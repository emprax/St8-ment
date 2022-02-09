using System.Collections.Concurrent;

namespace St8Ment.States
{
    public class StateReducerCore<TSubject> : IStateReducerCore<TSubject> where TSubject : ExtendedStateSubject<TSubject>
    {
        private readonly ConcurrentDictionary<StateId, IActionProvider<TSubject>> states;

        public StateReducerCore(ConcurrentDictionary<StateId, IActionProvider<TSubject>> states) => this.states = states;

        public bool TryGet(StateId stateId, out IActionProvider<TSubject> actionProvider)
        {
            if (this.states.TryGetValue(stateId, out var provider))
            {
                actionProvider = provider;
                return true;
            }

            actionProvider = null;
            return false;
        }
    }
}
