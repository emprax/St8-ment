using System.Threading;
using System.Threading.Tasks;
using St8Ment.States;

namespace St8Ment.Example.Console.States.Utilities
{
    public class StartAction : IAction
    {
        public StartAction(string text) => this.Text = text;

        public string Text { get; }
    }

    public class StartActionHandler : IActionHandler<StartAction, ExampleContext>
    {
        public Task Execute(StartAction action, IStateHandle<ExampleContext> state) => Task.Run(() =>
        {
            System.Console.WriteLine("  - Action arrived in start-action-handler. Action content: {0}.", action?.Text);
            if (string.IsNullOrWhiteSpace(action?.Text))
            {
                System.Console.WriteLine("    + Faulted result");
                state.Transition(ExampleState.Fault);

                return;
            }

            if (action.Text.Contains("Revoke"))
            {
                System.Console.WriteLine("    + Revoked result");
                state.Transition(ExampleState.Revoked);

                return;
            }

            System.Console.WriteLine("    + Started result");
            state.Transition(ExampleState.New);
        });
    }

    public class RevokeActionHandler : IActionHandler<RevokeAction, ExampleContext>
    {
        public Task Execute(RevokeAction action, IStateHandle<ExampleContext> state) => Task.Run(() =>
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

    public class PublishActionHandler : IActionHandler<PublishAction, ExampleContext>
    {
        public Task Execute(PublishAction action, IStateHandle<ExampleContext> state) => Task.Run(() =>
        {
            System.Console.WriteLine("  - Publish action arrived in start-action-handler. Action dispatched at: {0}.", action?.At);
            System.Console.WriteLine("    + Published result");

            state.Transition(ExampleState.Published);
        });
    }
}
