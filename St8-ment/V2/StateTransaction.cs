namespace St8_ment.V2
{
    public class StateTransaction<TAction, TState> : IStateTransaction<TAction, TState>
        where TAction : IAction
        where TState : IState
    {
        public StateTransaction(TAction action, TState state)
        {
            this.Action = action;
            this.State = state;
        }

        public TAction Action { get; }

        public TState State { get; }
    }
}