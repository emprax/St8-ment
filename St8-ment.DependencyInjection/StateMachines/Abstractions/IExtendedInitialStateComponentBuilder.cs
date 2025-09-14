using System;

namespace St8Ment.DependencyInjection.StateMachines.Abstractions;

public interface IExtendedInitialStateComponentBuilder
{
    IExtendedStateComponentCollectionBuilder ForInitial(StateId stateId, Action<IExtendedStateComponentBuilder> configuration);

    IExtendedStateComponentCollectionBuilder ForInitial(IExtendedStateConfiguration configuration);
}
