using Microsoft.Extensions.DependencyInjection;
using St8_ment.DependencyInjection.V1;
using St8_ment.V1;
using System.Threading;
using System.Threading.Tasks;

namespace St8_ment.Example.Console.V1
{
    public static class CaseV1
    {
        public static async Task Execute()
        {
            var stateMachine = new ServiceCollection()
                .AddStateMachine<ExampleContext>(builder =>
                {
                    builder.For<InitialState>(configurator => configurator.On<StartAction>().Transition<StartTransitioner>());
                    builder.For<ProcessingState>(configurator => configurator.On<StopAction>().Transition<StopTransitioner>());
                    builder.For<DoneState>();
                })
                .BuildServiceProvider()
                .GetRequiredService<IStateMachine<ExampleContext>>();

            var context = new ExampleContext();
            var state = stateMachine.Apply<InitialState>(context);

            var is_valid_1 = await context.Accept(new StartAction(), CancellationToken.None);
            var is_valid_2 = await context.Accept(new StopAction(), CancellationToken.None);
            var is_valid_3 = await context.Accept(new StopAction(), CancellationToken.None);

            Assert(is_valid_1);     // Should succeed
            Assert(is_valid_2);     // Should succeed
            Assert(is_valid_3);     // Should fail
        }

        private static void Assert(bool condition)
        {
            System.Console.WriteLine($"{(condition ? "Successfull" : "Failed")} transition.");
        }
    }
}
