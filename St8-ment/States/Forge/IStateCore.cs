namespace St8Ment.States.Forge
{
    public interface IStateCore
    {
        IActionHandler GetHandler<TAction>() where TAction : class, IAction;
    }
}