using System.Collections.Concurrent;

namespace St8Ment.States
{
    public class StateReducerCore<TContext> : IStateReducerCore<TContext> where TContext : class, IStateContext<TContext>
    {
        private readonly ConcurrentDictionary<StateId, IActionProvider<TContext>> states;

        public StateReducerCore(ConcurrentDictionary<StateId, IActionProvider<TContext>> states) => this.states = states;

        public bool TryGet(StateId stateId, out IActionProvider<TContext> actionProvider)
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
