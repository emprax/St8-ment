using System;
using System.Threading.Tasks;

namespace St8_ment.StateMachines.Components
{
    public class CallbackComponent : IItemStateComponent
    {
        private readonly Func<object> callback;
        private IStateComponent next;

        public CallbackComponent(Func<object> callback)
            => this.callback = callback;

        public void Add(IStateComponent component)
            => this.next = component;

        public async Task<StateTransitionResponse> Apply<TInput>(TInput input, StateId id)
        {
            if (this.callback?.Invoke() is ITransitionCallback<TInput> callback)
            {
                try
                {
                    await callback.Execute(input);
                }
                catch (Exception exception)
                {
                    return new StateTransitionResponse(StateMachineResponse.ToException(exception), id);
                }
            }

            var response = StateMachineResponse.ToUnspecified(id.Name, typeof(TInput).Name);
            return await (this.next?.Apply(input, id) ?? ToResponse(response, id));
        }

        private static Task<StateTransitionResponse> ToResponse(StateMachineResponse response, StateId id)
            => Task.FromResult(new StateTransitionResponse(response, id));
    }
}
