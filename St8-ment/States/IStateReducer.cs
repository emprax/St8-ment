namespace St8Ment.States
{
    public interface IStateReducer<TSubject> where TSubject : ExtendedStateSubject<TSubject>
    {
        bool TryGetProvider(StateId id, out IActionProvider<TSubject> provider);

        void SetState(StateId stateId, TSubject subject);
    }
}
