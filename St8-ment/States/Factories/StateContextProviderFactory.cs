using St8Ment.States.Abstractions.Builders;
using St8Ment.States.Abstractions.Core;
using St8Ment.States.Abstractions.Factories;
using St8Ment.States.Builders;
using St8Ment.States.Core;
using System;
using System.Collections.Generic;

namespace St8Ment.States.Factories;

public class StateContextProviderFactory : IStateContextProviderFactory
{
    public IStateContextProvider<TSubject> Create<TSubject>(Action<IStateContextsBuilder<TSubject>> action) where TSubject : class, IStateSubject<TSubject>
    {
        var dictionary = new Dictionary<string, IStateContext<TSubject>>();
        action.Invoke(new StateContextsBuilder<TSubject>(dictionary));

        return new StateContextProvider<TSubject>(dictionary);
    }
}