using St8Ment.States;

namespace St8Ment.Tests.Units.Utilities
{
    delegate void StateOutputCallback<TContext>(StateId id, out IActionProvider<TContext> provider) where TContext : class, IStateContext<TContext>;

    delegate void ActionOutputCallback<TAction, TContext>(out IActionHandler<TAction, TContext> handler)
        where TAction : class, IAction
        where TContext : class, IStateContext<TContext>;
}
