using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace St8_ment.StateMachines.Components
{
    public class StateComponent : IKeyValueStateComponent<string>
    {
        public IDictionary<string, IStateComponent> Components { get; }

        public StateComponent()
            => this.Components = new ConcurrentDictionary<string, IStateComponent>();

        public void Add(string key, IStateComponent component)
            => this.Components.Add(key, component);

        public bool TryGetValue(string key, out IStateComponent component)
            => this.Components.TryGetValue(key, out component);

        public async Task<StateTransitionResponse> Apply<TInput>(TInput input, StateId id)
        {
            if (this.Components.TryGetValue(typeof(TInput).FullName, out var component) && component != null)
            {
                var result = await component.Apply(input, id);
                if (result?.Response?.Succeeded ?? false)
                {
                    return result;
                }
            }
                
            if (this.Components.TryGetValue(typeof(object).FullName, out var defaultComponent) && defaultComponent != null)
            {
                var response = await defaultComponent.Apply(input, id);
            
                return response?.Response?.Succeeded ?? false
                    ? new StateTransitionResponse(StateMachineResponse.ToDefaultTransition(response.State?.Name, typeof(TInput).Name), response.State)
                    : response;
            }

            return new StateTransitionResponse(StateMachineResponse.ToUnspecified(id.Name, typeof(TInput).Name), id);
        }
    }
}
