using St8Ment.States.Abstractions.Core;
using System.Threading;
using System.Threading.Tasks;

namespace St8Ment.Tests.Units.Utilities;

public class CloseActionHandler : IActionHandler<TestStateSubject, CloseAction>
{
    public Task ExecuteAsync(CloseAction action, IStateHandle<TestStateSubject> stateHandle, CancellationToken cancellationToken)
    {
        stateHandle.Transition("CLOSED");
        return Task.CompletedTask;
    }
}