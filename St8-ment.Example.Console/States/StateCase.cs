using Microsoft.Extensions.DependencyInjection;
using St8Ment.DependencyInjection.States;
using St8Ment.Example.Console.States.Utilities;
using St8Ment.States.Abstractions.Factories;
using System.Threading;
using System.Threading.Tasks;

namespace St8Ment.Example.Console.States;

public class StateCase
{
    public static async Task Execute()
    {
        var provider = new ServiceCollection()
            .AddStateReducerFactory<ExampleContext>(builder =>
            {
                builder
                    .State(ExampleState.Fault)
                    .State(ExampleState.Start, bldr => bldr.Action<StartAction, StartActionHandler>())
                    .State(ExampleState.Published, bldr => bldr.Action<RevokeAction, RevokeActionHandler>())
                    .State(ExampleState.New, bldr =>
                    {
                        bldr.Action<PublishAction, PublishActionHandler>();
                        bldr.Action<RevokeAction, RevokeActionHandler>();
                    })
                    .State(ExampleState.Revoked, bldr =>
                    {
                        bldr.Action<PublishAction, PublishActionHandler>();
                        bldr.Action<StartAction, StartActionHandler>();
                    });
            })
            .BuildServiceProvider();

        var context = new ExampleContext(ExampleState.Start);
        var reducer = provider
            .GetRequiredService<IStateReducerFactory<ExampleContext>>()
            .Create(context);

        await reducer.ExecuteAsync(new StartAction("Hello"), CancellationToken.None);
        await reducer.ExecuteAsync(new PublishAction(), CancellationToken.None);
        await reducer.ExecuteAsync(new RevokeAction("Reasons"), CancellationToken.None);
        await reducer.ExecuteAsync(new PublishAction(), CancellationToken.None);
    }
}
