using System;

namespace St8_ment.DependencyInjection.StateMachines.Builders
{
    public interface IStateComponentCollectionBuilder
    {
        IStateComponentCollectionBuilder For(StateId stateId, Action<IStateComponentBuilder> configuration);
    }
}
