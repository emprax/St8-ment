using System;

namespace St8Ment.DependencyInjection.StateMachines.Builders
{
    public interface IStateComponentCollectionBuilder
    {
        IStateComponentCollectionBuilder For(StateId stateId, Action<IStateComponentBuilder> configuration);

        IStateComponentCollectionBuilder For(IStateConfiguration configuration);
    }
}
