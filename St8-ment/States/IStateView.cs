namespace St8Ment.States
{
    public interface IStateView<TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        TSubject Subject { get; }

        StateId StateId { get; }
    }
}
