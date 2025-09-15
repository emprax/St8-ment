using St8Ment.DependencyInjection.States.Abstractions;
using St8Ment.States.Abstractions.Core;
using St8Ment.States.Builders;
using St8Ment.States.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace St8Ment.DependencyInjection.States.Builders;

#pragma warning disable IDE0290 // Use primary constructor
internal class ExtendedStateContextsBuilder<TSubject> : IExtendedStateContextsBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    private readonly IDictionary<string, IStateContext<TSubject>> contexts;
    private readonly IDependencyFactory factory;

    public ExtendedStateContextsBuilder(IDictionary<string, IStateContext<TSubject>> contexts, IDependencyFactory factory)
    {
        this.contexts = contexts;
        this.factory = factory;
    }

    public IExtendedStateContextsBuilder<TSubject> State(StateId state, Action<IExtendedStateContextBuilder<TSubject>> action)
    {
        var dictionary = new Dictionary<int, IActionHandler<TSubject>>();
        var builder = new StateContextBuilder<TSubject>(dictionary);

        action.Invoke(new ExtendedStateContextBuilder<TSubject>(builder, this.factory));

        var context = new StateContext<TSubject>(dictionary);
        if (!this.contexts.TryAdd(state.Value, context))
        {
            this.contexts[state.Value] = context;
        }

        return this;
    }

    public IExtendedStateContextsBuilder<TSubject> State(StateId state)
    {
        var context = new StateContext<TSubject>(new Dictionary<int, IActionHandler<TSubject>>());
        if (!this.contexts.TryAdd(state.Value, context))
        {
            this.contexts[state.Value] = context;
        }

        return this;
    }
}