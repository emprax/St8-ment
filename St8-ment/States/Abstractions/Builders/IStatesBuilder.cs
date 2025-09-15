using St8Ment.States.Abstractions.Core;
using System;

namespace St8Ment.States.Abstractions.Builders;

public interface IStatesBuilder
{
    IStatesBuilder For<TSubject>(Action<IStateContextsBuilder<TSubject>> action) where TSubject : class, IStateSubject<TSubject>;
}