using System;

namespace St8Ment
{
    public abstract class StateId : IEquatable<StateId>
    {
        protected StateId(string name) => Name = name;

        public override bool Equals(object obj) => (obj is StateId stateId) && (stateId?.Name?.Equals(this.Name) ?? false);

        public override int GetHashCode() => this.Name.GetHashCode();

        public override string ToString() => this.Name;

        public string Name { get; }

        public bool Equals(StateId other) => other?.Name?.Equals(this.Name) ?? false;
    }
}
