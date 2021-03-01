using System;

namespace St8_ment.DependencyInjection.StateMachines.Builders
{
    public interface IInitialStateComponentBuilder
    {
        IStateComponentCollectionBuilder ForInitial(StateId stateId, Action<IStateComponentBuilder> configuration);
    }
}
