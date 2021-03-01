using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using St8_ment.States;

namespace St8_ment.Tests.Integration.States
{
    public class Test1ActionHandler : IActionHandler<Test1Action, TestContext>
    {
        private readonly ILogger<TestContext> logger;

        public Test1ActionHandler(ILogger<TestContext> logger) => this.logger = logger;

        public Task<StateId> Execute(Test1Action action, IStateView<TestContext> state)
        {
            this.logger.LogInformation("Test1-Action");
            return Task.FromResult<StateId>(TestStateId.Processing);
        }
    }

    public class Test2ActionHandler : IActionHandler<Test2Action, TestContext>
    {
        private readonly ILogger<TestContext> logger;

        public Test2ActionHandler(ILogger<TestContext> logger) => this.logger = logger;

        public Task<StateId> Execute(Test2Action action, IStateView<TestContext> state)
        {
            this.logger.LogInformation("Test2-Action");
            return Task.FromResult<StateId>(TestStateId.Fault);
        }
    }

    public class Test3ActionHandler : IActionHandler<Test3Action, TestContext>
    {
        private readonly ILogger<TestContext> logger;

        public Test3ActionHandler(ILogger<TestContext> logger) => this.logger = logger;

        public Task<StateId> Execute(Test3Action action, IStateView<TestContext> state)
        {
            this.logger.LogInformation("Test3-Action");
            return Task.FromResult<StateId>(TestStateId.Complete);
        }
    }
}
