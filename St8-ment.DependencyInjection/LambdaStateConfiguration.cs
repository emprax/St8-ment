using System;

namespace St8_ment.DependencyInjection
{
    public class LambdaStateConfiguration<TState, TContext> : StateConfiguration<TState, TContext>
        where TState : class, IState<TContext>
        where TContext : IStateContext
    {
        private readonly Action<IStateConfigurator<TState, TContext>> configuration;

        public LambdaStateConfiguration(Action<IStateConfigurator<TState, TContext>> configuration)
        {
            this.configuration = configuration;
        }

        protected override void Configure(IStateConfigurator<TState, TContext> configurator)
        {
            this.configuration.Invoke(configurator);
        }
    }
}