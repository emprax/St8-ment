namespace St8Ment.States
{
    public interface IStateContext<TContext> where TContext : class, IStateContext<TContext>
    {
        void SetState(IState<TContext> state);
    }
}
