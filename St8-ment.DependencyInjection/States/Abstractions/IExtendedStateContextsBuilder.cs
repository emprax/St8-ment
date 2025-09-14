using St8Ment;
using St8Ment.States.Abstractions.Core;

namespace St8ment.DependencyInjection.Abstractions;

public interface IExtendedStateContextsBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    IExtendedStateContextsBuilder<TSubject> State(StateId state, Action<IExtendedStateContextBuilder<TSubject>> action);
}