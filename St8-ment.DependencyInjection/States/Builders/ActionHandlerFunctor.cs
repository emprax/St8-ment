using St8Ment.DependencyInjection.States.Abstractions;
using St8Ment.States.Abstractions.Core;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace St8Ment.DependencyInjection.States.Builders;

#pragma warning disable IDE0290 // Use primary constructor
internal class ActionHandlerFunctor<TSubject, TAction> : IActionHandlerFunctor<TSubject, TAction>
    where TSubject : class, IStateSubject<TSubject>
    where TAction : IAction<TSubject>
{
    private readonly ConstructorInfo constructor;
    private readonly List<ParameterInfo> parameters;

    public ActionHandlerFunctor(ConstructorInfo constructor, ParameterInfo[] parameters)
    {
        this.constructor = constructor;
        this.parameters = [.. parameters];
    }

    public IActionHandler<TSubject, TAction> Invoke(IDependencyFactory factory)
    {
        var span = CollectionsMarshal.AsSpan(this.parameters);
        ref var searchSpace = ref MemoryMarshal.GetReference(span);

        var parameters = new object?[span.Length];
        for (var index = 0; index < span.Length; index++)
        {
            var parameterType = Unsafe.Add(ref searchSpace, index).ParameterType;
            parameters[index] = factory.Create(parameterType);
        }

        return (IActionHandler<TSubject, TAction>)this.constructor.Invoke(parameters);
    }
}