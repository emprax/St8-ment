using St8_ment.States;

namespace St8_ment.DependencyInjection.States
{
    public interface IActionBuilder<TAction, TContext>
        where TAction : class, IAction
        where TContext : class, IStateContext<TContext>
    {
        IStateBuilder<TContext> Handle<THandler>() where THandler : class, IActionHandler<TAction, TContext>;
    }
}
