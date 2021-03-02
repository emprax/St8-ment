using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using St8Ment.DependencyInjection.StateMachines;
using St8Ment.Example.Console.StateMachines.Utilities;
using St8Ment.StateMachines;

namespace St8Ment.Example.Console.StateMachines
{
    public class StateMachineCase
    {
        public static async Task Execute()
        {
            var provider = new ServiceCollection()
                .AddStateMachine((builder, _) =>
                {
                    builder
                        .ForInitial(ExampleState.Start, bldr => 
                        {
                            bldr.On<string>().WithGuard<ApplyRequestSpec>().To(ExampleState.New)
                                .OnDefault().To(ExampleState.Fault);
                        })
                        .For(ExampleState.New, bldr =>
                        {
                            bldr.On<string>().WithGuard(i => i.Contains("Updating")).WithCallback<ExampleCallback>().To(ExampleState.Updating)
                                .On<string>().WithGuard(i => i.Contains("Deleting")).To(ExampleState.Revoked)
                                .OnDefault().WithCallback<FailureCallback>().To(ExampleState.Fault);
                        })
                        .For(ExampleState.Updating, bldr =>
                        {
                            bldr.On<string>().WithGuard(i => i.Contains("Finished")).WithCallback<ExampleCallback>().To(ExampleState.Complete)
                                .On<string>().WithGuard(i => i.Contains("Deleting")).To(ExampleState.Revoked)
                                .OnDefault().WithCallback<FailureCallback>().To(ExampleState.Fault);
                        });
                })
                .BuildServiceProvider();

            System.Console.WriteLine("--- Happy Flow -----------");
            await HappyFlow(provider);

            System.Console.WriteLine("\n--- Direct Failure -----------");
            await DirectFailure(provider);

            System.Console.WriteLine("\n--- No-more-states Failure -----------");
            await NoMoreStatesFailure(provider);
        }

        private static async Task HappyFlow(IServiceProvider provider)
        {
            var stateMachine = provider.GetRequiredService<IStateMachine>();
            
            var result1 = await stateMachine.Apply("start");
            System.Console.WriteLine("Processed transfer to '{0}': {1}", stateMachine.Current.Name, result1.Message);

            var result2 = await stateMachine.Apply("Updating");
            System.Console.WriteLine("Processed transfer to '{0}': {1}", stateMachine.Current.Name, result2.Message);

            var result3 = await stateMachine.Apply("Finished");
            System.Console.WriteLine("Processed transfer to '{0}': {1}", stateMachine.Current.Name, result3.Message);
        }

        private static async Task DirectFailure(IServiceProvider provider)
        {
            var stateMachine = provider.GetRequiredService<IStateMachine>();
            
            var result = await stateMachine.Apply(string.Empty);
            System.Console.WriteLine("Processed transfer to '{0}': {1}", stateMachine.Current.Name, result.Message);
        }

        private static async Task NoMoreStatesFailure(IServiceProvider provider)
        {
            var stateMachine = provider.GetRequiredService<IStateMachine>();
            
            var result = await stateMachine.Apply("start");
            System.Console.WriteLine("Processed transfer to '{0}': {1}", stateMachine.Current.Name, result.Message);

            var result2 = await stateMachine.Apply("Deleting");
            System.Console.WriteLine("Processed transfer to '{0}': {1}", stateMachine.Current.Name, result2.Message);

            var result3 = await stateMachine.Apply("Updating");
            System.Console.WriteLine("Processed transfer to '{0}': {1}", stateMachine.Current.Name, result3.Message);
        }
    }
}
