using Moq;
using St8_ment.V2;
using System.Threading.Tasks;
using Xunit;

namespace St8_ment.Tests.V2.StateMachine
{
    public partial class StateMachineTests
    {
        public class TheApplyMethod : StateMachineTests
        {
            [Fact]
            public async Task ShouldReturnFalseWhenResultsForStatesCannotBeFound()
            {
                // Act
                var result = await this.stateMachine
                    .For(new TestState(new FakeContext()))
                    .Apply(new TestAction("Message"));

                // Assert
                Assert.False(result);
            }

            [Fact]
            public async Task ShouldReturnFalseWhenResultingProviderIsNull()
            {
                // Arrange
                this.registrations.Add(typeof(TestState).GetHashCode(), null);

                // Act
                var result = await this.stateMachine
                    .For(new TestState(new FakeContext()))
                    .Apply(new TestAction("Message"));

                // Assert
                Assert.False(result);
            }

            [Fact]
            public async Task ShouldReturnFalseWhenTransitionerIsNull()
            {
                // Arrange
                var stateTransitionerProvider = Mock.Of<IStateTransitionerProvider<TestState, FakeContext>>();
                var state = new TestState(new FakeContext());

                Mock.Get(stateTransitionerProvider)
                    .Setup(x => x.Find<TestAction>())
                    .Returns(default(IStateTransitioner<TestAction, TestState, FakeContext>));

                this.registrations.Add(typeof(TestState).GetHashCode(), stateTransitionerProvider);

                // Act
                var result = await this.stateMachine
                    .For(new TestState(new FakeContext()))
                    .Apply(new TestAction("Message"));

                // Assert
                Assert.False(result);

                Mock.Get(stateTransitionerProvider)
                    .Verify(x => x.Find<TestAction>(), Times.Once);
            }

            [Fact]
            public async Task ShouldReturnTrue()
            {
                // Arrange
                var stateTransitionerProvider = Mock.Of<IStateTransitionerProvider<TestState, FakeContext>>();
                var stateTransitioner = Mock.Of<IStateTransitioner<TestAction, TestState, FakeContext>>();
                var state = new TestState(new FakeContext());

                Mock.Get(stateTransitionerProvider)
                    .Setup(x => x.Find<TestAction>())
                    .Returns(stateTransitioner);

                Mock.Get(stateTransitioner)
                    .Setup(x => x.Apply(It.IsAny<IStateTransaction<TestAction, TestState>>()))
                    .ReturnsAsync(true);

                this.registrations.Add(typeof(TestState).GetHashCode(), stateTransitionerProvider);

                // Act
                var result = await this.stateMachine
                    .For(new TestState(new FakeContext()))
                    .Apply(new TestAction("Message"));

                // Assert
                Assert.True(result);

                Mock.Get(stateTransitionerProvider)
                    .Verify(x => x.Find<TestAction>(), Times.Once);

                Mock.Get(stateTransitioner)
                    .Verify(x => x.Apply(
                        It.IsAny<IStateTransaction<TestAction, TestState>>()),
                        Times.Once);
            }
        }
    }
}
