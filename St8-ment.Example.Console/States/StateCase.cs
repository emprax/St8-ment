using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using St8_ment.DependencyInjection.States;
using St8_ment.Example.Console.States.Utilities;
using St8_ment.States;

namespace St8_ment.Example.Console.States
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

            await context.ApplyAnAction(new StartAction("Hello"));
            await context.ApplyAnAction(new PublishAction());
            await context.ApplyAnAction(new RevokeAction("Reasons"));
            await context.ApplyAnAction(new PublishAction());
        }
    }
}
