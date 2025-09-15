using St8Ment.States.Abstractions.Core;

namespace St8Ment.States.Abstractions.Factories;

public interface IStateReducerFactory<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    IStateReducer<TSubject> Create(TSubject subject);
}