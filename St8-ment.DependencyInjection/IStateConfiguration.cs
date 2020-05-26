using Microsoft.Extensions.DependencyInjection;
using System;

namespace St8_ment.DependencyInjection
{
    public interface IStateConfiguration<out TState, TContext> 
        where TState : class, IState<TContext>
        where TContext : IStateContext
    {
        Func<IServiceProvider, TState> Build(IServiceCollection services);
    }
}