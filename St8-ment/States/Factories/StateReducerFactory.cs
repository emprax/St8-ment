using St8Ment.States.Abstractions.Core;
using St8Ment.States.Abstractions.Factories;
using St8Ment.States.Core;

namespace St8Ment.States.Factories;

public class StateReducerFactory<TSubject>(IStateContextProvider<TSubject> provider) : IStateReducerFactory<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    public IStateReducer<TSubject> Create(TSubject subject) => new StateReducer<TSubject>(provider, subject);
}

public class StateReducerFactory(IStateContextsProvider provider) : IStatesFactory
{
    public IStateReducer<TSubject>? Create<TSubject>(TSubject subject, StateId state) where TSubject : class, IStateSubject<TSubject>
    {
        var context = provider.Get<TSubject>();
        return context is not null
            ? new StateReducer<TSubject>(context, subject)
            : null;
    }
}