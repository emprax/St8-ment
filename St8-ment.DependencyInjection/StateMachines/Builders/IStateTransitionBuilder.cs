using System;
using System.Linq.Expressions;
using SpeciFire;
using St8_ment.StateMachines;

namespace St8_ment.DependencyInjection.StateMachines.Builders
{
    public interface IStateTransitionBuilder<TInput>
    {
        ISpecComponentBuilder<TInput> WithGuard(Func<IServiceProvider, ISpec<TInput>> guardFactory);
        
        ISpecComponentBuilder<TInput> WithGuard(ISpec<TInput> spec);

        ISpecComponentBuilder<TInput> WithGuard(Expression<Func<TInput, bool>> expression);

        ISpecComponentBuilder<TInput> WithGuard<TSpec>() where TSpec : class, ISpec<TInput>;

        ISpecComponentBuilder<TInput> WithGuard(Type specType);

        ICallbackComponentBuilder<TInput> WithCallback(Func<IServiceProvider, ITransitionCallback<TInput>> callbackFactory);

        ICallbackComponentBuilder<TInput> WithCallback(ITransitionCallback<TInput> callback);

        ICallbackComponentBuilder<TInput> WithCallback<TCallback>() where TCallback : class, ITransitionCallback<TInput>;

        ICallbackComponentBuilder<TInput> WithCallback(Type callbackType);

        IStateComponentBuilder To(StateId stateId);
    }
}
