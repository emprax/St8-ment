namespace St8_ment.States
{
    public interface IStateView<TContext> where TContext : class, IStateContext<TContext>
    {
        TContext Context { get; }

        StateId Id { get; }
    }
}
