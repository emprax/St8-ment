using St8Ment.States.Abstractions.Core;
using System;

namespace St8Ment.DependencyInjection.States.Abstractions;

public interface IExtendedStatesBuilder
{
    IExtendedStatesBuilder For<TSubject>(Action<IExtendedStateContextsBuilder<TSubject>> action) where TSubject : class, IStateSubject<TSubject>;
}