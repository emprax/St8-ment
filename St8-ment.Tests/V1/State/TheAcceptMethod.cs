using Moq;
using St8_ment.V1;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace St8_ment.Tests.V1.State
{
    public partial class StateTests
    {
        public class TheAcceptMethod : StateTests
        {
            [Fact]
            public async Task ShouldReturnFalseWhenTransitionerCannotBeFoundForAction()
            {
                // Arrange
                var action = new TestAction("Hello from test-action");

                Mock.Get(this.stateTransitionerProvider)
                    .Setup(s => s.Find<StateTransaction<TestAction, TestState>>())
                    .Returns(default(IStateTransitioner<StateTransaction<TestAction, TestState>>));

                // Act
                var result = await this.state.Accept(action, CancellationToken.None);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public async Task ShouldTrueWhenStateTransitionerIsFound()
            {
                // Arrange
                var action = new TestAction("Hello from test-action");
                var transitioner = Mock.Of<IStateTransitioner<StateTransaction<TestAction, TestState>>>(MockBehavior.Strict);

                Mock.Get(transitioner)
                    .Setup(t => t.Handle(It.IsAny<StateTransaction<TestAction, TestState>>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                Mock.Get(this.stateTransitionerProvider)
                    .Setup(s => s.Find<StateTransaction<TestAction, TestState>>())
                    .Returns(transitioner);

                // Act
                var result = await this.state.Accept(action, CancellationToken.None);

                // Assert
                Assert.True(result);

                Mock.Get(transitioner)
                    .Verify(t => t.Handle(
                        It.Is<StateTransaction<TestAction, TestState>>(x => x.Action == action && x.State == this.state),
                        It.IsAny<CancellationToken>()),
                        Times.Once);
            }
        }
    }
}
