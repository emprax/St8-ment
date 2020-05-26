namespace St8_ment
{
    public interface IAction { }

    public interface IAction<out TState> : IAction where TState : class, IState { }
}