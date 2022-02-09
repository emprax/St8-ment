namespace St8Ment.Tests.Units.Utilities
{
    public class TestStateId : StateId
    {
        public static readonly TestStateId New = new(1, "NEW");
        public static readonly TestStateId Processing = new(2, "PROCESSING");
        public static readonly TestStateId Complete = new(3, "COMPLETE");
        public static readonly TestStateId Fault = new(4, "FAULT");

        private TestStateId(uint id, string name) : base(name) => this.Id = id;

        public uint Id { get; }
    }
}
