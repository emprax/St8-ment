using Microsoft.Extensions.DependencyInjection;
using St8Ment.DependencyInjection.States;
using St8Ment.States;
using St8Ment.States.Abstractions.Factories;
using St8Ment.Tests.Units.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace St8Ment.Tests.Integration.States;

public class StateReducerTests
{
    private readonly IServiceProvider provider;

    public StateReducerTests() => this.provider = new ServiceCollection()
        .AddStateReducerFactory<TestStateSubject>(builder => builder
            .State("OPEN", b => b.Action<CloseAction, CloseActionHandler>())
            .State("CLOSED", b => b.Action<OpenAction, OpenActionHandler>()))
        .BuildServiceProvider();

    [Fact]
    public async Task ShouldTransitionInState()
    {
        // Arrange
        var subject = new TestStateSubject(new StateId("OPEN"));
        var reducer = this.provider
            .GetRequiredService<IStateReducerFactory<TestStateSubject>>()
            .Create(subject);

        // Act
        var response = await reducer.ExecuteAsync(new CloseAction(), CancellationToken.None);

        // Assert
        Assert.Equal(StateResponseType.SUCCESS, response.Type);
        Assert.Equal("CLOSED", subject.State.Id);
    }
}
