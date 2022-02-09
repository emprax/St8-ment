using System;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States.Forge
{
    public interface IStateForgeCoreBuilder<out TSubject> where TSubject : StateSubject
    {
        IStateForgeCoreBuilder<TSubject> For(StateId stateId);

        IStateForgeCoreBuilder<TSubject> For(IStateForgeStateConfiguration<TSubject> configuration);

        IStateForgeCoreBuilder<TSubject> For(StateId stateId, Action<IStateForgeStateBuilder<TSubject>> configuration);
    }
}