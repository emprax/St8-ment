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
                    builder.For<InitialState>(configurator => configurator.On<StartAction>().Transition<StartTransition>());
                    builder.For<ProcessingState>(configurator => configurator.On<StopAction>().Transition<StopTransition>());
                })
                .BuildServiceProvider()
                .GetRequiredService<IStateMachine<ExampleContext>>();

            var state = stateMachine.Find<InitialState>();
            var context = new ExampleContext(state);

            await context.Apply(new StartAction(), CancellationToken.None);
            await context.Apply(new StopAction(), CancellationToken.None);
        }
    }
}
