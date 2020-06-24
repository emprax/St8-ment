using Microsoft.Extensions.DependencyInjection;
using St8_ment.DependencyInjection.V2;
using St8_ment.V2;
using System.Threading.Tasks;

namespace St8_ment.Example.Console.V2
{
    public static class CaseV2
    {
        public static async Task Execute()
        {
            var stateMachine = new ServiceCollection()
                .AddStateMachineV2<ExampleContext>(builder =>
                {
                    builder.For<InitialState>(configurator => configurator.On<StartAction>().Transition<StartTransitioner>());
                    builder.For<ProcessingState>(configurator => configurator.On<StopAction>().Transition<StopTransitioner>());
                    builder.For<DoneState>();
                })
                .BuildServiceProvider()
                .GetRequiredService<IStateMachine<ExampleContext>>();

            var context = new ExampleContext();
            context.SetState(new InitialState(context));
            
            var is_valid_1 = await context.Apply(stateMachine, new StartAction());
            var is_valid_2 = await context.Apply(stateMachine, new StopAction());
            var is_valid_3 = await context.Apply(stateMachine, new StopAction());

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
