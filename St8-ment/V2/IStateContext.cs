namespace St8_ment.V2
{
    public interface IStateContext<TContext> where TContext : IStateContext<TContext>
    {
        IState<TContext> State { get; }

        void SetState<TState>(TState state) where TState : class, IState<TContext>;
    }
}
