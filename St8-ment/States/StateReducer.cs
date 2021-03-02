namespace St8Ment.States
{
    public class StateReducer<TSubject> : IStateReducer<TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        private readonly IStateReducerCore<TSubject> core;

        public StateReducer(IStateReducerCore<TSubject> core) => this.core = core;

        public bool TryGetProvider(StateId id, out IActionProvider<TSubject> provider)
            => this.core.TryGet(id, out provider);

        public void SetState(StateId stateId, TSubject subject)
            => subject.SetState(new State<TSubject>(stateId, subject, this));
    }
}
