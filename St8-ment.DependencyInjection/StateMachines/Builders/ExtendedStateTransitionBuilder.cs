using SpeciFire;
using SpeciFire.Specifications;
using St8Ment.DependencyInjection.StateMachines.Abstractions;
using St8Ment.StateMachines;
using St8Ment.StateMachines.Components;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace St8Ment.DependencyInjection.StateMachines.Builders;

internal class ExtendedStateTransitionBuilder<TInput> : IExtendedStateTransitionBuilder<TInput>
{
    private readonly IExtendedStateComponentBuilder builder;
    private readonly IItemStateComponent parent;
    private readonly IDependencyFactory factory;

    public ExtendedStateTransitionBuilder(IExtendedStateComponentBuilder builder, IItemStateComponent parent, IDependencyFactory factory)
    {
        this.builder = builder;
        this.parent = parent;
        this.factory = factory;
    }

    public IExtendedStateComponentBuilder To(StateId stateId)
    {
        this.parent.Add(new ResultComponent(stateId));
        return this.builder;
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
                .Select(p => provider.Create(p.ParameterType))?
                .ToArray();

            return constructor?.Invoke(parameters) as ITransitionCallback<TInput>;
        });
    }

    public IExtendedSpecComponentBuilder<TInput> WithGuard(Func<IDependencyFactory, ISpec<TInput>?> guardFactory)
    {
        var component = new SpecComponent(() => guardFactory.Invoke(this.factory));
        this.parent.Add(component);

        return new ExtendedSpecComponentBuilder<TInput>(this.builder, component, this.factory);
    }

    public IExtendedSpecComponentBuilder<TInput> WithGuard(ISpec<TInput> spec) => this.WithGuard(_ => spec);

    public IExtendedSpecComponentBuilder<TInput> WithGuard(Expression<Func<TInput, bool>> expression) => this.WithGuard(new ExpressionSpec<TInput>(expression));

    public IExtendedSpecComponentBuilder<TInput> WithGuard<TSpec>() where TSpec : class, ISpec<TInput> => this.WithGuard(typeof(TSpec));

    public IExtendedSpecComponentBuilder<TInput> WithGuard(Type specType)
    {
        var type = typeof(ISpec<TInput>);
        if (!type.IsAssignableFrom(specType) && !specType.IsAssignableFrom(type))
        {
            throw new NotSupportedException($"The type '{specType.FullName}' is not assignable from required type '{type.FullName}'");
        }

        return this.WithGuard(provider =>
        {
            var constructor = specType?
                .GetConstructors()?
                .FirstOrDefault();

            var parameters = constructor?
                .GetParameters()?
                .Select(p => provider.Create(p.ParameterType))?
                .ToArray();

            return constructor?.Invoke(parameters) as ISpec<TInput>;
        });
    }
}
