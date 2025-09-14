using System;

namespace St8Ment.StateMachines.Builders;

public interface IInitialStateComponentBuilder
{
    IStateComponentCollectionBuilder ForInitial(StateId stateId, Action<IStateComponentBuilder> configuration);

    IStateComponentCollectionBuilder ForInitial(IStateConfiguration configuration);
}
