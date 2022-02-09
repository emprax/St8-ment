using Microsoft.Extensions.DependencyInjection;
using St8Ment.DependencyInjection.States;
using St8Ment.Example.Console.States.Utilities;
using System.Threading.Tasks;
using St8Ment.States.Forge;

namespace St8Ment.Example.Console.States
{
    public class StateForgeCase
    {
        public static async Task Execute()
        {
            var provider = new ServiceCollection()
                .AddStateForge((builder, _) =>
                {
                    builder.Connect<ExampleContext>(coreBuilder =>
                    {
                        coreBuilder
                            .For(ExampleState.Fault)
                            .For(ExampleState.Start, bldr =>
                            {
                                bldr.On<StartAction>().Handle<StartActionHandler>();
                            })
                            .For(ExampleState.New, bldr =>
                            {
                                bldr.On<PublishAction>().Handle<PublishActionHandler>();
                                bldr.On<RevokeAction>().Handle<RevokeActionHandler>();
                            })
                            .For(ExampleState.Revoked, bldr =>
                            {
                                bldr.On<PublishAction>().Handle<PublishActionHandler>();
                                bldr.On<StartAction>().Handle<StartActionHandler>();
                            })
                            .For(ExampleState.Published, bldr =>
                            {
                                bldr.On<RevokeAction>().Handle<RevokeActionHandler>();
                            });
                    });
                })
                .BuildServiceProvider();

            var forge = provider.GetRequiredService<IStateForge>();
            var context = new ExampleContext(ExampleState.Start);
            
            await forge.Connect(context).Apply(new StartAction("Hello"));
            await forge.Connect(context).Apply(new PublishAction());
            await forge.Connect(context).Apply(new RevokeAction("Reasons"));
            await forge.Connect(context).Apply(new PublishAction());
        }
    }
}
