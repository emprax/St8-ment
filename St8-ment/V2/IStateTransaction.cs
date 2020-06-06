namespace St8_ment.V2
{
    public interface IStateTransaction<TAction, TState>
        where TAction : IAction
        where TState : IState
    {
        TAction Action { get; }

        TState State { get; }
    }
}