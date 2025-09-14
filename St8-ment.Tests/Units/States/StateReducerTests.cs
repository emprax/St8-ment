using Moq;
using St8Ment;
using St8Ment.States;
using St8Ment.States.Abstractions.Core;
using St8Ment.States.Core;
using St8Ment.Tests.Units.Utilities;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace St8Ment.Tests.Units.States;

public class StateReducerTests
{
    private readonly Mock<IActionHandler<TestStateSubject, TestAction>> handler;
    private readonly Mock<IStateContextProvider<TestStateSubject>> provider;
    private readonly Mock<IStateContext<TestStateSubject>> context;
    private readonly StateReducer<TestStateSubject> reducer;

    public StateReducerTests()
    {
        this.provider = new(MockBehavior.Strict);
        this.context = new(MockBehavior.Strict);
        this.handler = new(MockBehavior.Strict);
        this.reducer = new(this.provider.Object, new(new StateId("OPEN")));
    }

    [Fact]
    public async Task ShouldTransitionFromState()
    {
        // Arrange
        this.provider
            .Setup(x => x.Get(It.Is<StateId>(y => y.Value == "OPEN")))
            .Returns(this.context.Object);

        this.context
            .Setup(x => x.Get<TestAction>())
            .Returns(this.handler.Object);

        this.handler
            .Setup(x => x.ExecuteAsync(
                It.IsAny<TestAction>(),
                It.IsAny<IStateHandle<TestStateSubject>>(),
                It.IsAny<CancellationToken>()))
            .Callback<TestAction, IStateHandle<TestStateSubject>, CancellationToken>((_, x, _) => x.Transition(new StateId("CLOSED")))
            .Returns(Task.CompletedTask);

        // Act
        var result = await this.reducer.ExecuteAsync(new TestAction("ACTION"), CancellationToken.None);

        // Assert
        Assert.Equal(StateResponseType.SUCCESS, result.Type);
        Assert.Equal("CLOSED", this.reducer.Subject.State.Id.Value);
    }

    [Fact]
    public async Task ShouldReturnNoStateWhenContextNotFound()
    {
        // Arrange
        this.provider
            .Setup(x => x.Get(It.Is<StateId>(y => y.Value == "OPEN")))
            .Returns(default(IStateContext<TestStateSubject>));

        // Act
        var result = await this.reducer.ExecuteAsync(new TestAction("ACTION"), CancellationToken.None);

        // Assert
        Assert.Equal(StateResponseType.NOSTATE, result.Type);
        Assert.Equal("OPEN", this.reducer.Subject.State.Id.Value);
    }

    [Fact]
    public async Task ShouldReturnNoActionWhenHandlerNotFound()
    {
        // Arrange
        this.provider
            .Setup(x => x.Get(It.Is<StateId>(y => y.Value == "OPEN")))
            .Returns(this.context.Object);

        this.context
            .Setup(x => x.Get<TestAction>())
            .Returns(default(IActionHandler<TestStateSubject, TestAction>));

        // Act
        var result = await this.reducer.ExecuteAsync(new TestAction("ACTION"), CancellationToken.None);

        // Assert
        Assert.Equal(StateResponseType.NOACTION, result.Type);
        Assert.Equal("OPEN", this.reducer.Subject.State.Id.Value);
    }
}