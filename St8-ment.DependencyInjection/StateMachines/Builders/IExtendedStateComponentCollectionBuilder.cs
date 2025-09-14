using System;

namespace St8Ment.DependencyInjection.StateMachines.Builders;

public interface IExtendedStateComponentCollectionBuilder
{
    IExtendedStateComponentCollectionBuilder For(StateId stateId, Action<IExtendedStateComponentBuilder> configuration);

    IExtendedStateComponentCollectionBuilder For(IExtendedStateConfiguration configuration);
}
