namespace St8_ment.V1
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