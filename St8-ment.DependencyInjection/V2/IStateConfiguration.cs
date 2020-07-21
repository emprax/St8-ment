using Microsoft.Extensions.DependencyInjection;
using St8_ment.V2;
using System;

namespace St8_ment.DependencyInjection.V2
{
    public interface IStateConfiguration<TState, TContext>
        where TState : class, IState<TContext>
        where TContext : class, IStateContext<TContext>
    {
        Func<IServiceProvider, IStateTransitionerProvider<TState, TContext>> Build(IServiceCollection services);
    }
}
