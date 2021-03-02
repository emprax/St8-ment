namespace St8Ment.States
{
    public class StateReducer<TContext> : IStateReducer<TContext> where TContext : class, IStateContext<TContext>
    {
        private readonly IStateReducerCore<TContext> core;

        public StateReducer(IStateReducerCore<TContext> core) => this.core = core;

        public bool TryGetProvider(StateId id, out IActionProvider<TContext> provider)
            => this.core.TryGet(id, out provider);

        public void SetState(StateId stateId, TContext context)
            => context.SetState(new State<TContext>(stateId, context, this));
    }
}
