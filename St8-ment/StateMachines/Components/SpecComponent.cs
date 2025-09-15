using System;
using System.Threading.Tasks;
using SpeciFire;

namespace St8Ment.StateMachines.Components;

public class SpecComponent(Func<object?> specification) : IItemStateComponent
{
    private IStateComponent? next;

    public void Add(IStateComponent component) => this.next = component;

    public Task<StateTransitionResponse> Apply<TInput>(TInput input, StateId id)
    {
        var defaultResponse = StateMachineResponse.ToUnspecified(id.Value, typeof(TInput).Name);
        if (specification?.Invoke() is not ISpec<TInput> spec)
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
