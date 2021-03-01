using St8_ment.States;

namespace St8_ment.Tests.Units.States
{
    delegate void StateOutputCallback<TContext>(StateId id, out IActionProvider<TContext> provider) where TContext : class, IStateContext<TContext>;

    delegate void ActionOutputCallback<TAction, TContext>(out IActionHandler<TAction, TContext> handler)
        where TAction : class, IAction
        where TContext : class, IStateContext<TContext>;
}
