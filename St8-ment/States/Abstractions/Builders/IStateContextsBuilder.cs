using St8Ment.States.Abstractions.Core;
using System;

namespace St8Ment.States.Abstractions.Builders;

public interface IStateContextsBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    IStateContextsBuilder<TSubject> State(StateId state, Action<IStateContextBuilder<TSubject>> action);
}