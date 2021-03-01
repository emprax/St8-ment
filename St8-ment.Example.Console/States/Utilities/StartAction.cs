using System.Threading;
using System.Threading.Tasks;
using St8_ment.States;

namespace St8_ment.Example.Console.States.Utilities
{
    public class StartAction : IAction
    {
        public StartAction(string text) => this.Text = text;

        public string Text { get; }
    }

    public class StartActionHandler : IActionHandler<StartAction, ExampleContext>
    {
        public Task<StateId> Execute(StartAction action, IStateView<ExampleContext> state) => Task.Run<StateId>(() =>
        {
            System.Console.WriteLine("  - Action arrived in start-action-handler. Action content: {0}.", action?.Text);
            if (string.IsNullOrWhiteSpace(action?.Text))
            {
                System.Console.WriteLine("    + Faulted result");
                return ExampleState.Fault;
            }

            if (action.Text.Contains("Revoke"))
            {
                System.Console.WriteLine("    + Revoked result");
                return ExampleState.Revoked;
            }

            System.Console.WriteLine("    + Started result");
            return ExampleState.New;
        });
    }

    public class RevokeActionHandler : IActionHandler<RevokeAction, ExampleContext>
    {
        public Task<StateId> Execute(RevokeAction action, IStateView<ExampleContext> state) => Task.Run<StateId>(() =>
        {
            System.Console.WriteLine(
                "  - Revoke action arrived in start-action-handler. Action dispatched at: {0} for reason: {1}.",
                action?.At,
                action?.Reason);

            if (string.IsNullOrWhiteSpace(action?.Reason))
            {
                System.Console.WriteLine("    + Faulted result");
                return ExampleState.Fault;
            }

            System.Console.WriteLine("    + Revoked result");
            return ExampleState.Revoked;
        });
    }

    public class PublishActionHandler : IActionHandler<PublishAction, ExampleContext>
    {
        public Task<StateId> Execute(PublishAction action, IStateView<ExampleContext> state) => Task.Run<StateId>(() =>
        {
            System.Console.WriteLine("  - Publish action arrived in start-action-handler. Action dispatched at: {0}.", action?.At);
            
            System.Console.WriteLine("    + Published result");
            return ExampleState.Published;
        });
    }
}
