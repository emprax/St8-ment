using St8Ment.States.Abstractions.Core;

namespace St8Ment.States.Abstractions.Factories;

public interface IStatesFactory
{
    IStateReducer<TSubject>? Create<TSubject>(TSubject subject, StateId state) where TSubject : class, IStateSubject<TSubject>;
}