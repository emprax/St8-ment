using System.Threading.Tasks;
using Moq;
using St8Ment.StateMachines;
using St8Ment.StateMachines.Components;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.StateMachines
{
    public class StateComponentCollectionTests
    {
        private readonly IStateComponent next;
        private readonly IKeyValueStateComponent<StateId> component;

        public StateComponentCollectionTests()
        {
            this.next = Mock.Of<IStateComponent>(MockBehavior.Strict);
            this.component = new StateComponentCollection();
        }

        [Fact]
        public async Task ShouldReturnUnknownStateResponseWhenComponentCouldNotBeFound()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            // Act
            var result = await this.component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.UnknownState.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnUnknownStateResponseWhenComponentIsNull()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            this.component.Add(TestStateId.New, null);

            // Act
            var result = await this.component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.UnknownState.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnSuccess()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            this.component.Add(TestStateId.New, this.next);

            Mock.Get(this.next)
                .Setup(n => n.Apply(input, state))
                .ReturnsAsync(new StateTransitionResponse(StateMachineResponse.Success, state));

            // Act
            var result = await this.component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Success.Id, result.Response.Id);
        }
    }
}
