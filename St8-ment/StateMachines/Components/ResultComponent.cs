using System.Threading.Tasks;

namespace St8Ment.StateMachines.Components
{
    public class ResultComponent : IItemStateComponent
    {
        private readonly StateId stateId;
        private IStateComponent next;

        public ResultComponent(StateId stateId)
            => this.stateId = stateId;

        public void Add(IStateComponent component)
            => this.next = component;

        public Task<StateTransitionResponse> Apply<TInput>(TInput input, StateId id)
        {
            var response = StateMachineResponse.ToSuccess(id.Name, this.stateId.Name);
            return this.next?.Apply(input, this.stateId) ?? Task.FromResult(new StateTransitionResponse(response, this.stateId));
        }
    }
}
