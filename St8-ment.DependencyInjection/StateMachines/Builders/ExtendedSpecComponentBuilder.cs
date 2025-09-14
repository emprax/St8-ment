using St8Ment.StateMachines;
using St8Ment.StateMachines.Components;
using System;
using System.Linq;

namespace St8Ment.DependencyInjection.StateMachines.Builders;

internal class ExtendedSpecComponentBuilder<TInput> : IExtendedSpecComponentBuilder<TInput>
{
    private readonly IExtendedStateComponentBuilder builder;
    private readonly IItemStateComponent parent;
    private readonly IDependencyFactory factory;

    public ExtendedSpecComponentBuilder(IExtendedStateComponentBuilder builder, IItemStateComponent parent, IDependencyFactory factory)
    {
        this.builder = builder;
        this.parent = parent;
        this.factory = factory;
    }

    public IExtendedCallbackComponentBuilder<TInput> WithCallback(Func<IDependencyFactory, ITransitionCallback<TInput>?> callbackFactory)
    {
        var component = new CallbackComponent(() => callbackFactory.Invoke(this.factory));
        this.parent.Add(component);

        return new ExtendedCallbackComponentBuilder<TInput>(this.builder, component);
    }

    public IExtendedCallbackComponentBuilder<TInput> WithCallback(ITransitionCallback<TInput> callback) => this.WithCallback(_ => callback);

    public IExtendedCallbackComponentBuilder<TInput> WithCallback<TCallback>() where TCallback : class, ITransitionCallback<TInput> => this.WithCallback(typeof(TCallback));

    public IExtendedCallbackComponentBuilder<TInput> WithCallback(Type callbackType)
    {
        var type = typeof(ITransitionCallback<TInput>);
        if (!type.IsAssignableFrom(callbackType) && !callbackType.IsAssignableFrom(type))
        {
            throw new NotSupportedException($"The type '{callbackType.FullName}' is not assignable from required type '{type.FullName}'");
        }

        return this.WithCallback(provider =>
        {
            var constructor = callbackType?
                .GetConstructors()?
                .FirstOrDefault();

            var parameters = constructor?
                .GetParameters()?
                .Select(p => provider.Get(p.ParameterType))?
                .ToArray();

            return constructor?.Invoke(parameters) as ITransitionCallback<TInput>;
        });
    }

    public IExtendedStateComponentBuilder To(StateId stateId)
    {
        this.parent.Add(new ResultComponent(stateId));
        return this.builder;
    }
}
