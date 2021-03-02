using System.Threading.Tasks;
using Moq;
using SpeciFire;
using St8Ment.StateMachines;
using St8Ment.StateMachines.Components;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.StateMachines
{
    public class SpecComponentTests
    {
        private readonly ISpec<TestAction> spec;
        private readonly IStateComponent next;

        public SpecComponentTests()
        {
            this.next = Mock.Of<IStateComponent>(MockBehavior.Strict);
            this.spec = Mock.Of<ISpec<TestAction>>(MockBehavior.Strict);
        }

        [Fact]
        public async Task ShouldReturnUnspecifiedResponseWhenBothSpecFactoryAndNextComponentAreNull()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            var component = new SpecComponent(null);

            // Act
            var result = await component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Unspecified.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnUnspecifiedResponseWhenBothSpecAndNextComponentAreNull()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            var component = new SpecComponent(() => null);

            // Act
            var result = await component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Unspecified.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnUnspecifiedResponseWhenNextComponentIdNullAndSpecIsOfWrongType()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            var component = new SpecComponent(() => Mock.Of<ISpec<string>>(MockBehavior.Strict));

            // Act
            var result = await component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Unspecified.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnUnspecifiedResponseWhenNextComponentIdNull()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            var component = new SpecComponent(() => this.spec);

            Mock.Get(this.spec)
                .Setup(s => s.IsSatisfiedBy(input))
                .Returns(true);

            // Act
            var result = await component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Unspecified.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnUnsatisfiedResponseWhenSpecIsNotSatisfied()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            var component = new SpecComponent(() => this.spec);

            Mock.Get(this.spec)
                .Setup(s => s.IsSatisfiedBy(input))
                .Returns(false);

            // Act
            var result = await component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Unsatisfied.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnSuccess()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            var component = new SpecComponent(() => this.spec);
            component.Add(this.next);

            Mock.Get(this.spec)
                .Setup(s => s.IsSatisfiedBy(input))
                .Returns(true);

            Mock.Get(this.next)
                .Setup(n => n.Apply(input, state))
                .ReturnsAsync(new StateTransitionResponse(StateMachineResponse.Success, state));

            // Act
            var result = await component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Success.Id, result.Response.Id);
        }
    }
}
