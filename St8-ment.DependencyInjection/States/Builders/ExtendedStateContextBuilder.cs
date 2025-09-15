using St8Ment.DependencyInjection.States.Abstractions;
using St8Ment.States.Abstractions.Builders;
using St8Ment.States.Abstractions.Core;

namespace St8Ment.DependencyInjection.States.Builders;

#pragma warning disable IDE0290 // Use primary constructor
internal class ExtendedStateContextBuilder<TSubject> : IExtendedStateContextBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    private readonly IStateContextBuilder<TSubject> builder;
    private readonly IDependencyFactory factory;

    public ExtendedStateContextBuilder(IStateContextBuilder<TSubject> builder, IDependencyFactory factory)
    {
        this.builder = builder;
        this.factory = factory;
    }

    public IExtendedStateContextBuilder<TSubject> Action<TAction, THandler>()
        where TAction : IAction<TSubject>
        where THandler : IActionHandler<TSubject, TAction>
    {
        var constructor = typeof(THandler).GetConstructors()[0];
        var parameters = constructor.GetParameters();

        var functor = new ActionHandlerFunctor<TSubject, TAction>(constructor, parameters);
        var handler = new ActionFactoryHandler<TSubject, TAction>(this.factory, functor);

        this.builder.Action<TAction, ActionFactoryHandler<TSubject, TAction>>(handler);
        return this;
    }
}