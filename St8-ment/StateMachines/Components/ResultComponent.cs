using System.Threading.Tasks;

namespace St8Ment.StateMachines.Components;

public class ResultComponent(StateId stateId) : IItemStateComponent
{
    private IStateComponent? next;

    public void Add(IStateComponent component) => this.next = component;

    public Task<StateTransitionResponse> Apply<TInput>(TInput input, StateId id)
    {
        var response = StateMachineResponse.ToSuccess(id.Value, stateId.Value);
        return this.next?.Apply(input, stateId) ?? Task.FromResult(new StateTransitionResponse(response, stateId));
    }
}
