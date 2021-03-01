using St8_ment.States;

namespace St8_ment.DependencyInjection.States
{
    public interface IStateBuilder<TContext> where TContext : class, IStateContext<TContext>
    {
        IActionBuilder<TAction, TContext> On<TAction>() where TAction : class, IAction;
    }
}
