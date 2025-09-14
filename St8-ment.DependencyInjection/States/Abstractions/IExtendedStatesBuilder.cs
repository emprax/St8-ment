using St8Ment.States.Abstractions.Core;

namespace St8ment.DependencyInjection.Abstractions;

public interface IExtendedStatesBuilder
{
    IExtendedStatesBuilder For<TSubject>(Action<IExtendedStateContextsBuilder<TSubject>> action) where TSubject : class, IStateSubject<TSubject>;
}