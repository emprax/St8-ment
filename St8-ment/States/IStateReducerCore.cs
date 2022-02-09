namespace St8Ment.States
{
    public interface IStateReducerCore<TSubject> where TSubject : ExtendedStateSubject<TSubject>
    {
        bool TryGet(StateId stateId, out IActionProvider<TSubject> actionProvider);
    }
}
