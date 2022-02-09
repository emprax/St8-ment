using System;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States.Forge
{
    public interface IStateForgeBuilder
    {
        IStateForgeBuilder Connect<TSubject>(Action<IStateForgeCoreBuilder<TSubject>> configuration) where TSubject : StateSubject;
    }
}