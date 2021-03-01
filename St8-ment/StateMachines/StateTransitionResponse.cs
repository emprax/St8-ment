namespace St8_ment.StateMachines
{
    public class StateTransitionResponse
    {
        public StateTransitionResponse(StateMachineResponse response, StateId state)
        {
            this.Response = response;
            this.State = state;
        }

        public StateMachineResponse Response { get; }

        public StateId State { get; }
    }
}
