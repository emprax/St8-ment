namespace St8Ment.States.Abstractions.Core;

public interface IStateHandle<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    TSubject Subject { get; }

    void Transition(StateId state);
}