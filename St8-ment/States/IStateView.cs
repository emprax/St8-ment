namespace St8Ment.States
{
    public interface IStateView<TContext> where TContext : class, IStateContext<TContext>
    {
        TContext Context { get; }

        StateId StateId { get; }
    }
}
