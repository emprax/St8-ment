namespace St8_ment.Tests.Units.Utilities
{
    public class TestStateId : StateId
    {
        public static readonly TestStateId New = new TestStateId(1, "NEW");
        public static readonly TestStateId Processing = new TestStateId(2, "PROCESSING");
        public static readonly TestStateId Complete = new TestStateId(3, "COMPLETE");
        public static readonly TestStateId Fault = new TestStateId(4, "FAULT");

        private TestStateId(uint id, string name) : base(name)
        {
            Id = id;
            Name = name;
        }

        public uint Id { get; }

        public string Name { get; }
    }
}
