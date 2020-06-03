using Microsoft.Extensions.DependencyInjection;
using St8_ment.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace St8_ment.Example.Console
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            Assert(is_valid_1);     // Should be successful
            Assert(is_valid_2);     // Should be successful
            Assert(is_valid_3);     // Should be failure
        }

        private static void Assert(bool condition)
        {
            System.Console.WriteLine($"{(condition ? "Successfull" : "Failed")} transition.");
        }
    }
}
