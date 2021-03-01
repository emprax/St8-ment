namespace St8_ment.States
{
    public interface IActionProvider<TContext> where TContext : class, IStateContext<TContext>
    {
        bool TryGet<TAction>(out IActionHandler<TAction, TContext> actionHandler) where TAction : class, IAction;
    }
}
