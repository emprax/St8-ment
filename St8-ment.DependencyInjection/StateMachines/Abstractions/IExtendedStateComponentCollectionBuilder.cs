using System;

namespace St8Ment.DependencyInjection.StateMachines.Abstractions;

public interface IExtendedStateComponentCollectionBuilder
{
    IExtendedStateComponentCollectionBuilder For(StateId stateId, Action<IExtendedStateComponentBuilder> configuration);

    IExtendedStateComponentCollectionBuilder For(IExtendedStateConfiguration configuration);
}
