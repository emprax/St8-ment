using System;

namespace St8Ment.DependencyInjection.StateMachines.Builders
{
    public interface IInitialStateComponentBuilder
    {
        IStateComponentCollectionBuilder ForInitial(StateId stateId, Action<IStateComponentBuilder> configuration);

        IStateComponentCollectionBuilder ForInitial(IStateConfiguration configuration);
    }
}
