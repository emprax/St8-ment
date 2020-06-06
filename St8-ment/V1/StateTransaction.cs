namespace St8_ment.V1
{
    public class StateTransaction<TAction, TState> : ITransaction<TAction, TState>
        where TAction : IAction
        where TState : class, IState
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