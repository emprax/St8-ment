using System;

namespace St8Ment.StateMachines.Builders;

public interface IStateComponentCollectionBuilder
{
    IStateComponentCollectionBuilder For(StateId stateId, Action<IStateComponentBuilder> configuration);

    IStateComponentCollectionBuilder For(IStateConfiguration configuration);
}