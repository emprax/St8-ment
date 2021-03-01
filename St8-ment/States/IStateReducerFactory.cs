namespace St8_ment.States
{
    public interface IStateReducerFactory<TKey, TContext> where TContext : class, IStateContext<TContext>
    {
        IStateReducer<TContext> Create(TKey key);
    }
}
