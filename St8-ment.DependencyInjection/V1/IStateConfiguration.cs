using Microsoft.Extensions.DependencyInjection;
using St8_ment.V1;
using System;

namespace St8_ment.DependencyInjection.V1
{
    public interface IStateConfiguration<out TState, TContext> 
        where TState : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        Func<IServiceProvider, TContext, TState> Build(IServiceCollection services);
    }
}