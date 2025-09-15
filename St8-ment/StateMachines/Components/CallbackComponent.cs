using System;
using System.Threading.Tasks;

namespace St8Ment.StateMachines.Components;

public class CallbackComponent(Func<object?> callback) : IItemStateComponent
{
    private IStateComponent? next;

    public void Add(IStateComponent component) => this.next = component;

    public async Task<StateTransitionResponse> Apply<TInput>(TInput input, StateId id)
    {
        if (callback?.Invoke() is ITransitionCallback<TInput> transitionCallback)
        {
            try
            {
                await transitionCallback.Execute(input);
            }
            catch (Exception exception)
            {
                return new StateTransitionResponse(StateMachineResponse.ToException(exception), id);
            }
        }

        var response = StateMachineResponse.ToUnspecified(id.Value, typeof(TInput).Name);
        return await (this.next?.Apply(input, id) ?? ToResponse(response, id));
    }

    private static Task<StateTransitionResponse> ToResponse(StateMachineResponse response, StateId id)
        => Task.FromResult(new StateTransitionResponse(response, id));
}
