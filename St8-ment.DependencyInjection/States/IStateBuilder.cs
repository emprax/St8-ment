using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IStateBuilder<TContext> where TContext : class, IStateContext<TContext>
    {
        IActionBuilder<TAction, TContext> On<TAction>() where TAction : class, IAction;
    }
}
