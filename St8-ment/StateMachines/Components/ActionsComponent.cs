using System.Collections.Generic;
using System.Threading.Tasks;

namespace St8_ment.StateMachines.Components
{
    public class ActionsComponent : IItemStateComponent
    {
        private readonly IList<IStateComponent> components;

        public ActionsComponent()
            => this.components = new List<IStateComponent>();

        public void Add(IStateComponent component)
            => this.components.Add(component);

        public async Task<StateTransitionResponse> Apply<TInput>(TInput input, StateId id)
        {
            var response = StateMachineResponse.ToUnspecified(id.Name, typeof(TInput).Name);
            var defaultResult = new StateTransitionResponse(response, id);

            foreach (var component in components)
            {
                var result = await (component?.Apply(input, id) ?? Task.FromResult(defaultResult));
                if (result?.Response?.Succeeded ?? false)
                {
                    return result;
                }

                defaultResult = result;
            }
            
            return defaultResult;
        }
    }
}
