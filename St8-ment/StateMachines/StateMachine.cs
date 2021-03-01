using System;
using System.Threading.Tasks;
using St8_ment.StateMachines.Components;

namespace St8_ment.StateMachines
{
    public class StateMachine : IStateMachine
    {
        private readonly IStateComponent component;

        public StateMachine(IStateMachineCore core)
        {
            this.component = core.Component;
            this.Current = core.InitialStateId;
        }

        public StateId Current { get; private set; }

        public async Task<StateMachineResponse> Apply<TInput>(TInput action)
        {
            if (this.component is null)
            {
                throw new NotImplementedException("The state-machine is not implemented correctly.");
            }

            var result = await this.component.Apply(action, this.Current);
            this.Current = result?.State ?? this.Current;

            return result?.Response ?? StateMachineResponse.ToUnknownState(this.Current.Name);
        }
    }
}
