using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace St8Ment.StateMachines.Components
{
    public class StateComponentCollection : IKeyValueStateComponent<StateId>
    {
        public IDictionary<StateId, IStateComponent> States { get; }

        public StateComponentCollection()
            => this.States = new ConcurrentDictionary<StateId, IStateComponent>();

        public bool TryGetValue(StateId key, out IStateComponent component)
            => this.States.TryGetValue(key, out component);

        public void Add(StateId key, IStateComponent component)
            => this.States.Add(key, component);

        public Task<StateTransitionResponse> Apply<TInput>(TInput input, StateId id)
        {
            var defaultResponse = StateMachineResponse.ToUnknownState(id.Name);

            return (!this.States.TryGetValue(id, out var component) || component is null)
                ? Task.FromResult(new StateTransitionResponse(defaultResponse, id))
                : component.Apply(input, id);
        }
    }
}
