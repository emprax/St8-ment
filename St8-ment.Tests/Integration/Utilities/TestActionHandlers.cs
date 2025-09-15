using Microsoft.Extensions.Logging;
using St8Ment.States.Abstractions.Core;
using System.Threading;
using System.Threading.Tasks;

namespace St8Ment.Tests.Integration.Utilities;

public class Test1ActionHandler(ILogger<TesTSubject> logger) : IActionHandler<TesTSubject, Test1Action>
{
    public Task ExecuteAsync(Test1Action action, IStateHandle<TesTSubject> state, CancellationToken cancellationToken)
    {
        logger.LogInformation("Test1-Action");
        state.Transition(TestStateId.Processing);

        return Task.CompletedTask;
    }
}

public class Test2ActionHandler(ILogger<TesTSubject> logger) : IActionHandler<TesTSubject, Test2Action>
{
    public Task ExecuteAsync(Test2Action action, IStateHandle<TesTSubject> state, CancellationToken cancellationToken)
    {
        logger.LogInformation("Test2-Action");
        state.Transition(TestStateId.Fault);

        return Task.CompletedTask;
    }
}

public class Test3ActionHandler(ILogger<TesTSubject> logger) : IActionHandler<TesTSubject, Test3Action>
{
    public Task ExecuteAsync(Test3Action action, IStateHandle<TesTSubject> state, CancellationToken cancellationToken)
    {
        logger.LogInformation("Test3-Action");
        state.Transition(TestStateId.Complete);

        return Task.CompletedTask;
    }
}
