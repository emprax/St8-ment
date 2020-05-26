namespace St8_ment
{
    public interface ITransaction { }

    public interface ITransaction<TAction, TState> : ITransaction
        where TAction : IAction
        where TState : class, IState
    {
        TAction Action { get; }

        TState State { get; }
    }
}