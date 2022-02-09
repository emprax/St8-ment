using System;

namespace St8Ment.States
{
    public class StateResponse : IEquatable<StateResponse>
    {
        public static readonly StateResponse Success = new(1, "SUCCESS", "Successfully applied the provided action to the state.");
        public static readonly StateResponse StateActionsNotFound = new(2, "STATE_ACTIONS_NOT_FOUND", "Could not find any actions for current state.");
        public static readonly StateResponse NoMatchingAction = new(3, "NO_MATCHING_ACTION", "The current state has no match for provided action.");

        private StateResponse(uint id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        public uint Id { get; }

        public string Name { get; }

        public string Description { get; }

        public bool Equals(StateResponse other) => this.Id == other?.Id && this.Name == other?.Name;

        public static StateResponse ToSuccess(StateId id, string action)
            => new(1, "SUCCESS", $"Successfully applied action '{action}' to state '{id.Name}'.");

        public static StateResponse ToStateActionsNotFound(StateId id)
            => new(2, "STATE_ACTIONS_NOT_FOUND", $"Could not find any actions for current state '{id.Name}'.");

        public static StateResponse ToNoMatchingAction(StateId id, string action)
            => new(3, "NO_MATCHING_ACTION", $"The current state '{id.Name}' has no match for action '{action}'.");
    }
}
