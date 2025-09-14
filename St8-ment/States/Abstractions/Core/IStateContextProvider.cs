namespace St8Ment.States.Abstractions.Core;

public interface IStateContextProvider;

public interface IStateContextProvider<TSubject> : IStateContextProvider where TSubject : class, IStateSubject<TSubject>
{
    IStateContext<TSubject>? Get(StateId state);
}