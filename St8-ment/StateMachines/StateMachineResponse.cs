using System;

namespace St8Ment.StateMachines
{
    public class StateMachineResponse : IEquatable<StateMachineResponse>
    {
        public static readonly StateMachineResponse Success = new StateMachineResponse(1, true, "Successfully made the transition");
        public static readonly StateMachineResponse Unspecified = new StateMachineResponse(2, false, "The provided input was not recognized by the machine for current state");
        public static readonly StateMachineResponse Unsatisfied = new StateMachineResponse(3, false, "The specification was unsuccessful in regards to the provide input");
        public static readonly StateMachineResponse UnknownState = new StateMachineResponse(4, false, "The provided state identification could not find the corresponding state");
        public static readonly StateMachineResponse Exception = new StateMachineResponse(5, false, "An exception was thrown");
        public static readonly StateMachineResponse DefaultTransition = new StateMachineResponse(6, true, "Default transition applied");

        private StateMachineResponse(uint id, bool succeeded, string message)
        {
            this.Id = id;
            this.Succeeded = succeeded;
            this.Message = message;
        }

        public uint Id { get; }

        public bool Succeeded { get; }

        public string Message { get; }

        public bool Equals(StateMachineResponse other) => this.Id == other?.Id && this.Succeeded == other?.Succeeded;

        public static StateMachineResponse ToSuccess(string fromState, string toState)
            => new StateMachineResponse(1, true, $"{Success.Message} from state '{fromState}' to state '{toState}'");

        public static StateMachineResponse ToUnspecified(string currentState, string input)
            => new StateMachineResponse(2, false, $"{Unspecified.Message} '{currentState}'. Input: {input}");

        public static StateMachineResponse ToUnsatisfied(string input)
            => new StateMachineResponse(3, false, $"{Unsatisfied.Message} '{input}'");

        public static StateMachineResponse ToUnknownState(string stateId)
            => new StateMachineResponse(4, false, $"{UnknownState.Message}. Provided state-id: {stateId}");

        public static StateMachineResponse ToException(Exception exception)
            => new StateMachineResponse(5, false, $"{Exception.Message}: {exception.GetType()}. Message: {exception.Message}");

        public static StateMachineResponse ToDefaultTransition(string stateId, string input)
            => new StateMachineResponse(6, true, $"{DefaultTransition.Message} with input '{input}'. Transitioned into state: {stateId}.");
    }
}
