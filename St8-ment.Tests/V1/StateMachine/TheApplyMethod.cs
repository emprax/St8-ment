using Moq;
using St8_ment.V1;
using Xunit;

namespace St8_ment.Tests.V1.StateMachine
{
    public partial class StateMachineTestsV1
    {
        public class TheApplyMethod : StateMachineTestsV1
        {
            [Fact]
            public void ShouldReturnFalseWhenResultsForStatesCannotBeFound()
            {
                // Act & Assert
                Assert.False(this.stateMachine.Apply<TestState>(new FakeContext()));
            }

            [Fact]
            public void ShouldReturnFalseWhenResultingProviderIsNull()
            {
                // Arrange
                this.registrations.Add(typeof(TestState).GetHashCode(), null);

                // Act & Assert
                Assert.False(this.stateMachine.Apply<TestState>(new FakeContext()));
            }

            [Fact]
            public void ShouldReturnFalseWhenResultingProviderResultIsNull()
            {
                // Arrange
                this.registrations.Add(typeof(TestState).GetHashCode(), _ => null);

                // Act & Assert
                Assert.False(this.stateMachine.Apply<TestState>(new FakeContext()));
            }

            [Fact]
            public void ShouldReturnTrue()
            {
                // Arrange
                var stateTransitionerProvider = Mock.Of<IStateTransitionerProvider>();
                var context = new FakeContext();
                var state = new TestState(context, stateTransitionerProvider);

                this.registrations.Add(typeof(TestState).GetHashCode(), _ => state);

                // Act
                var result = this.stateMachine.Apply<TestState>(context);

                // Assert
                Assert.True(result);
                Assert.NotNull(context.State);
                Assert.IsType<TestState>(context.State);
            }
        }
    }
}
