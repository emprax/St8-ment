using St8Ment.States.Abstractions.Core;
using System.Threading;
using System.Threading.Tasks;

namespace St8Ment.Example.Console.States.Utilities;

public class StartAction : IAction<ExampleContext>
{
    public StartAction(string text) => this.Text = text;

    public string Text { get; }
}

public class StartActionHandler : IActionHandler<ExampleContext, StartAction>
{
    public async Task ExecuteAsync(StartAction action, IStateHandle<ExampleContext> state, CancellationToken cancellationToken)
    {
        await System.Console.Out.WriteLineAsync($"  - Action arrived in start-action-handler. Action content: {action?.Text}.");
        if (string.IsNullOrWhiteSpace(action?.Text))
        {
            await System.Console.Out.WriteLineAsync("    + Faulted result");
            state.Transition(ExampleState.Fault);

            return;
        }

        if (action.Text.Contains("Revoke"))
        {
            await System.Console.Out.WriteLineAsync("    + Revoked result");
            state.Transition(ExampleState.Revoked);

            return;
        }

        await System.Console.Out.WriteLineAsync("    + Started result");
        state.Transition(ExampleState.New);
    }
}

public class RevokeActionHandler : IActionHandler<ExampleContext, RevokeAction>
{
    public Task ExecuteAsync(RevokeAction action, IStateHandle<ExampleContext> state, CancellationToken cancellationToken) => Task.Run(() =>
    {
        System.Console.WriteLine(
            "  - Revoke action arrived in start-action-handler. Action dispatched at: {0} for reason: {1}.",
            action?.At,
            action?.Reason);

        if (string.IsNullOrWhiteSpace(action?.Reason))
        {
            System.Console.WriteLine("    + Faulted result");
            state.Transition(ExampleState.Fault);

            return;
        }

        System.Console.WriteLine("    + Revoked result");
        state.Transition(ExampleState.Revoked);
    });
}

public class PublishActionHandler : IActionHandler<ExampleContext, PublishAction>
{
    public Task ExecuteAsync(PublishAction action, IStateHandle<ExampleContext> state, CancellationToken cancellationToken) => Task.Run(() =>
    {
        System.Console.WriteLine("  - Publish action arrived in start-action-handler. Action dispatched at: {0}.", action?.At);
        System.Console.WriteLine("    + Published result");

        state.Transition(ExampleState.Published);
    });
}
