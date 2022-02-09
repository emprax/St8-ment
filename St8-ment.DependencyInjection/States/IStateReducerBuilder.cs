using System;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IStateReducerBuilder<TSubject> where TSubject : ExtendedStateSubject<TSubject>
    {
        IStateReducerBuilder<TSubject> For(StateId stateId);

        IStateReducerBuilder<TSubject> For(StateId stateId, Action<IStateBuilder<TSubject>> configuration);

        IStateReducerBuilder<TSubject> For(IStateConfiguration<TSubject> configuration);
    }
}
