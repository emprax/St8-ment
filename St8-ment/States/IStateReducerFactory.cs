namespace St8Ment.States
{
    public interface IStateReducerFactory<TKey, TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        IStateReducer<TSubject> Create(TKey key);
    }
}
