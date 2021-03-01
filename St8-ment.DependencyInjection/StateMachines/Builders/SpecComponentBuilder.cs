using System;
using System.Linq;
using St8_ment.StateMachines;
using St8_ment.StateMachines.Components;

namespace St8_ment.DependencyInjection.StateMachines.Builders
{
    internal class SpecComponentBuilder<TInput> : ISpecComponentBuilder<TInput>
    {
        private readonly IStateComponentBuilder builder;
        private readonly IItemStateComponent parent;
        private readonly IServiceProvider provider;

        public SpecComponentBuilder(IStateComponentBuilder builder, IItemStateComponent parent, IServiceProvider provider)
        {
            this.builder = builder;
            this.parent = parent;
            this.provider = provider;
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

        public IStateComponentBuilder To(StateId stateId)
        {
            this.parent.Add(new ResultComponent(stateId));
            return this.builder;
        }
    }
}
