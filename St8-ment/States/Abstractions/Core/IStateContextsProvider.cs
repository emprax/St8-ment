namespace St8Ment.States.Abstractions.Core;

public interface IStateContextsProvider
{
    IStateContextProvider<TSubject>? Get<TSubject>() where TSubject : class, IStateSubject<TSubject>;
}