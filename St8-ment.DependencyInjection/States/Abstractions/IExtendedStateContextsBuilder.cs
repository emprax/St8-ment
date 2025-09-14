using St8Ment.States.Abstractions.Core;
using System;

namespace St8Ment.DependencyInjection.States.Abstractions;

public interface IExtendedStateContextsBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    IExtendedStateContextsBuilder<TSubject> State(StateId state, Action<IExtendedStateContextBuilder<TSubject>> action);

    IExtendedStateContextsBuilder<TSubject> State(StateId state);
}