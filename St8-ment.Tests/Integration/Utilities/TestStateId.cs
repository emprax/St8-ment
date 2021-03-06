﻿namespace St8Ment.Tests.Integration.Utilities
{
    public class TestStateId : StateId
    {
        public static readonly TestStateId New = new TestStateId(1, "NEW");
        public static readonly TestStateId Processing = new TestStateId(2, "PROCESSING");
        public static readonly TestStateId Complete = new TestStateId(3, "COMPLETE");
        public static readonly TestStateId Fault = new TestStateId(4, "FAULT");

        private TestStateId(uint id, string name) : base(name)
        {
            this.Id = id;
        }

        public uint Id { get; }
    }
}
