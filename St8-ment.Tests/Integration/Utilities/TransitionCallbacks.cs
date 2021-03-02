using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using St8Ment.StateMachines;

namespace St8Ment.Tests.Integration.Utilities
{
    public class Test1Callback : ITransitionCallback<Test1Action>
    {
        private readonly ILogger<TestContext> logger;

        public Test1Callback(ILogger<TestContext> logger) => this.logger = logger;

        public Task Execute(Test1Action action)
        {
            this.logger.LogInformation(action.ActionName);
            return Task.CompletedTask;
        }
    }

    public class Test2Callback : ITransitionCallback<Test2Action>
    {
        private readonly ILogger<TestContext> logger;

        public Test2Callback(ILogger<TestContext> logger) => this.logger = logger;

        public Task Execute(Test2Action action)
        {
            this.logger.LogInformation(action.ActionName);
            return Task.CompletedTask;
        }
    }

    public class Test3Callback : ITransitionCallback<Test3Action>
    {
        private readonly ILogger<TestContext> logger;

        public Test3Callback(ILogger<TestContext> logger) => this.logger = logger;

        public Task Execute(Test3Action action)
        {
            this.logger.LogInformation(action.ActionName);
            return Task.CompletedTask;
        }
    }
}
