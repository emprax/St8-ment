using St8Ment.States.Abstractions.Builders;
using St8Ment.States.Abstractions.Core;
using System;

namespace St8Ment.States.Abstractions.Factories;

public interface IStateContextProviderFactory
{
    IStateContextProvider<TSubject> Create<TSubject>(Action<IStateContextsBuilder<TSubject>> action) where TSubject : class, IStateSubject<TSubject>;
}