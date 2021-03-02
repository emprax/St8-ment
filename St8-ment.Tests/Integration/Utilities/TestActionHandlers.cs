using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using St8Ment.States;

namespace St8Ment.Tests.Integration.Utilities
{
    public class Test1ActionHandler : IActionHandler<Test1Action, TesTSubject>
    {
        private readonly ILogger<TesTSubject> logger;

        public Test1ActionHandler(ILogger<TesTSubject> logger) => this.logger = logger;

        public Task<StateId> Execute(Test1Action action, IStateView<TesTSubject> state)
        {
            logger.LogInformation("Test1-Action");
            return Task.FromResult<StateId>(TestStateId.Processing);
        }
    }

    public class Test2ActionHandler : IActionHandler<Test2Action, TesTSubject>
    {
        private readonly ILogger<TesTSubject> logger;

        public Test2ActionHandler(ILogger<TesTSubject> logger) => this.logger = logger;

        public Task<StateId> Execute(Test2Action action, IStateView<TesTSubject> state)
        {
            logger.LogInformation("Test2-Action");
            return Task.FromResult<StateId>(TestStateId.Fault);
        }
    }

    public class Test3ActionHandler : IActionHandler<Test3Action, TesTSubject>
    {
        private readonly ILogger<TesTSubject> logger;

        public Test3ActionHandler(ILogger<TesTSubject> logger) => this.logger = logger;

        public Task<StateId> Execute(Test3Action action, IStateView<TesTSubject> state)
        {
            logger.LogInformation("Test3-Action");
            return Task.FromResult<StateId>(TestStateId.Complete);
        }
    }
}
