using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace St8Ment.States.Forge
{
    public class State<TSubject> : IState, IStateHandle<TSubject> where TSubject : StateSubject
    {
        private readonly IStateForgeCore core;

        public State(IStateForgeCore core, TSubject subject)
        {
            this.core = core;
            this.Subject = subject;
        }

        public TSubject Subject { get; }

        public void Transition(StateId nextState) => this.Subject.StateId = nextState;

        public async Task<StateResponse> Apply<TAction>(TAction action) where TAction : class, IAction
        {
            var stateCore = this.core.GetForState(this.Subject.StateId);
            if (stateCore is null)
            {
                return StateResponse.ToStateActionsNotFound(this.Subject.StateId);
            }

            if (stateCore.GetHandler<TAction>() is not IActionHandler<TAction, TSubject> handler)
            {
                return StateResponse.ToNoMatchingAction(this.Subject.StateId, nameof(TAction));
            }
            
            await handler.Execute(action, this);
            return StateResponse.ToSuccess(this.Subject.StateId, nameof(TAction));
        }
    }
}
