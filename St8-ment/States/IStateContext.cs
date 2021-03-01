namespace St8_ment.States
{
    public interface IStateContext<TContext> where TContext : class, IStateContext<TContext>
    {
        void SetState(IState<TContext> state);
    }
}
