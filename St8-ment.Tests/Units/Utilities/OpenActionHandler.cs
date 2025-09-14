using St8Ment.States.Abstractions.Core;
using System.Threading;
using System.Threading.Tasks;

namespace St8Ment.Tests.Units.Utilities;

public class OpenActionHandler : IActionHandler<TestStateSubject, OpenAction>
{
    public Task ExecuteAsync(OpenAction action, IStateHandle<TestStateSubject> stateHandle, CancellationToken cancellationToken)
    {
        stateHandle.Transition("OPEN");
        return Task.CompletedTask;
    }
}