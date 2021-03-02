using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using St8Ment.DependencyInjection.States;
using St8Ment.Example.Console.States.Utilities;
using St8Ment.States;

namespace St8Ment.Example.Console.States
{
    public class StateCase
    {
        public static async Task Execute()
        {
            var provider = new ServiceCollection()
                .AddStateReducer<ExampleContext>((builder, _) =>
                {
                    builder
                        .For(ExampleState.Fault)
                        .For(ExampleState.Start, bldr =>
                        {
                            bldr.On<StartAction>().Handle<StartActionHandler>();
                        })
                        .For(ExampleState.New, bldr =>
                        {
                            bldr.On<PublishAction>().Handle<PublishActionHandler>()
                                .On<RevokeAction>().Handle<RevokeActionHandler>();
                        })
                        .For(ExampleState.Revoked, bldr =>
                        {
                            bldr.On<PublishAction>().Handle<PublishActionHandler>()
                                .On<StartAction>().Handle<StartActionHandler>();
                        })
                        .For(ExampleState.Published, bldr =>
                        {
                            bldr.On<RevokeAction>().Handle<RevokeActionHandler>();
                        });
                })
                .BuildServiceProvider();

            var reducer = provider.GetRequiredService<IStateReducer<ExampleContext>>();
            var context = new ExampleContext();

            reducer.SetState(ExampleState.Start, context);

            await context.Apply(new StartAction("Hello"));
            await context.Apply(new PublishAction());
            await context.Apply(new RevokeAction("Reasons"));
            await context.Apply(new PublishAction());
        }
    }
}
