using System;
using System.Threading.Tasks;
using SpeciFire;

namespace St8Ment.StateMachines.Components
{
    public class SpecComponent : IItemStateComponent
    {
        private readonly Func<object> specification;
        private IStateComponent next;

        public SpecComponent(Func<object> specification)
            => this.specification = specification;

        public void Add(IStateComponent component)
            => this.next = component;

        public Task<StateTransitionResponse> Apply<TInput>(TInput input, StateId id)
        {
            var defaultResponse = StateMachineResponse.ToUnspecified(id.Name, typeof(TInput).Name);
            if (!(this.specification?.Invoke() is ISpec<TInput> spec))
            {
                return this.next?.Apply(input, id) ?? ToResponse(defaultResponse, id);
            }

            return spec.IsSatisfiedBy(input)
                ? this.next?.Apply(input, id) ?? ToResponse(defaultResponse, id)
                : ToResponse(StateMachineResponse.ToUnsatisfied(typeof(TInput).Name), id);
        }

        private static Task<StateTransitionResponse> ToResponse(StateMachineResponse response, StateId id)
            => Task.FromResult(new StateTransitionResponse(response, id));
    }
}
