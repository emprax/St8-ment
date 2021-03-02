using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IActionBuilder<TAction, TContext>
        where TAction : class, IAction
        where TContext : class, IStateContext<TContext>
    {
        IStateBuilder<TContext> Handle<THandler>() where THandler : class, IActionHandler<TAction, TContext>;
    }
}
