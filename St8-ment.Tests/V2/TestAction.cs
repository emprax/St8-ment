﻿namespace St8_ment.Tests.V2
{
    public class TestAction : IAction 
    {
        public string Message { get; }

        public TestAction(string message) => this.Message = message;
    }
}
