namespace St8Ment.States
{
    public interface IStateReducerFactory<TKey, TSubject> where TSubject : ExtendedStateSubject<TSubject>
    {
        IStateReducer<TSubject> Create(TKey key);
    }
}
