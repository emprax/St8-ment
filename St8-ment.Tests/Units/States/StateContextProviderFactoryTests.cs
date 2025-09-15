using St8Ment.States.Factories;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.States;

public class StateContextProviderFactoryTests
{
    private readonly StateContextProviderFactory factory;

    public StateContextProviderFactoryTests() => this.factory = new();

    [Fact]
    public void ShouldCreateProvider()
    {
        // Arrange
        var provider = this.factory.Create<TestStateSubject>(x => x
            .State("OPEN", b => b.Action<CloseAction, CloseActionHandler>(new()))
            .State("CLOSED", b => b.Action<OpenAction, OpenActionHandler>(new())));
        
        // Act
        var open = provider.Get("OPEN");
        var closed = provider.Get("CLOSED");

        // Assert
        Assert.NotNull(open);
        Assert.NotNull(closed);

        Assert.NotNull(open!.Get<CloseAction>());
        Assert.NotNull(closed!.Get<OpenAction>());
        Assert.Null(open!.Get<OpenAction>());
        Assert.Null(closed!.Get<CloseAction>());
    }
}
