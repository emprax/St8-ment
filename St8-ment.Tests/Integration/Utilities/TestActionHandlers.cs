using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using St8Ment.States;

namespace St8Ment.Tests.Integration.Utilities
{
    public class Test1ActionHandler : IActionHandler<Test1Action, TesTSubject>
    {
        private readonly ILogger<TesTSubject> logger;

        public Test1ActionHandler(ILogger<TesTSubject> logger) => this.logger = logger;

        public Task Execute(Test1Action action, IStateHandle<TesTSubject> state)
        {
            logger.LogInformation("Test1-Action");
            state.Transition(TestStateId.Processing);

            return Task.CompletedTask;
        }
    }

    public class Test2ActionHandler : IActionHandler<Test2Action, TesTSubject>
    {
        private readonly ILogger<TesTSubject> logger;

        public Test2ActionHandler(ILogger<TesTSubject> logger) => this.logger = logger;

        public Task Execute(Test2Action action, IStateHandle<TesTSubject> state)
        {
            logger.LogInformation("Test2-Action");
            state.Transition(TestStateId.Fault);

            return Task.CompletedTask;
        }
    }

    public class Test3ActionHandler : IActionHandler<Test3Action, TesTSubject>
    {
        private readonly ILogger<TesTSubject> logger;

        public Test3ActionHandler(ILogger<TesTSubject> logger) => this.logger = logger;

        public Task Execute(Test3Action action, IStateHandle<TesTSubject> state)
        {
            logger.LogInformation("Test3-Action");
            state.Transition(TestStateId.Complete);

            return Task.CompletedTask;
        }
    }
}
