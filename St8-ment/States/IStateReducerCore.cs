namespace St8Ment.States
{
    public interface IStateReducerCore<TContext> where TContext : class, IStateContext<TContext>
    {
        bool TryGet(StateId stateId, out IActionProvider<TContext> actionProvider);
    }
}
