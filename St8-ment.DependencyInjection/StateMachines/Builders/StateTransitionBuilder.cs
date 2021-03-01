using System;
using System.Linq;
using System.Linq.Expressions;
using SpeciFire;
using SpeciFire.Specifications;
using St8_ment.StateMachines;
using St8_ment.StateMachines.Components;

namespace St8_ment.DependencyInjection.StateMachines.Builders
{
    internal class StateTransitionBuilder<TInput> : IStateTransitionBuilder<TInput>
    {
        private readonly IStateComponentBuilder builder;
        private readonly IItemStateComponent parent;
        private readonly IServiceProvider provider;

        public StateTransitionBuilder(IStateComponentBuilder builder, IItemStateComponent parent, IServiceProvider provider)
        {
            this.builder = builder;
            this.parent = parent;
            this.provider = provider;
        }

        public IStateComponentBuilder To(StateId stateId)
        {
            this.parent.Add(new ResultComponent(stateId));
            return this.builder;
        }

        public ICallbackComponentBuilder<TInput> WithCallback(Func<IServiceProvider, ITransitionCallback<TInput>> callbackFactory)
        {
            var component = new CallbackComponent(() => callbackFactory.Invoke(this.provider));
            this.parent.Add(component);

            return new CallbackComponentBuilder<TInput>(this.builder, component);
        }

        public ICallbackComponentBuilder<TInput> WithCallback(ITransitionCallback<TInput> callback) => this.WithCallback(_ => callback);

        public ICallbackComponentBuilder<TInput> WithCallback<TCallback>() where TCallback : class, ITransitionCallback<TInput> => this.WithCallback(typeof(TCallback));

        public ICallbackComponentBuilder<TInput> WithCallback(Type callbackType)
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
                    .Select(p => provider.GetService(p?.ParameterType))?
                    .ToArray();

                return constructor?.Invoke(parameters) as ITransitionCallback<TInput>;
            });
        }

        public ISpecComponentBuilder<TInput> WithGuard(Func<IServiceProvider, ISpec<TInput>> guardFactory)
        {
            var component = new SpecComponent(() => guardFactory.Invoke(this.provider));
            this.parent.Add(component);

            return new SpecComponentBuilder<TInput>(this.builder, component, this.provider);
        }

        public ISpecComponentBuilder<TInput> WithGuard(ISpec<TInput> spec) => this.WithGuard(_ => spec);

        public ISpecComponentBuilder<TInput> WithGuard(Expression<Func<TInput, bool>> expression) => this.WithGuard(new ExpressionSpec<TInput>(expression));

        public ISpecComponentBuilder<TInput> WithGuard<TSpec>() where TSpec : class, ISpec<TInput> => this.WithGuard(typeof(TSpec));

        public ISpecComponentBuilder<TInput> WithGuard(Type specType)
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
                    .Select(p => provider.GetService(p?.ParameterType))?
                    .ToArray();

                return constructor?.Invoke(parameters) as ISpec<TInput>;
            });
        }
    }
}
