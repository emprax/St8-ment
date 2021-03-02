using System.Threading.Tasks;
using Moq;
using St8Ment.StateMachines;
using St8Ment.StateMachines.Components;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.StateMachines
{
    public class ActionsComponentTests
    {
        private readonly IStateComponent next;
        private readonly IItemStateComponent component;

        public ActionsComponentTests()
        {
            this.next = Mock.Of<IStateComponent>(MockBehavior.Strict);
            this.component = new ActionsComponent();
        }

        [Fact]
        public async Task ShouldReturnUnspecifiedResponseWhenComponentsListIsEmpty()
        {
            // Act
            var result = await this.component.Apply(new TestAction("TEST"), TestStateId.New);

            // Assert
            Assert.Equal(StateMachineResponse.Unspecified.Id, result.Response.Id);
            Assert.Equal(TestStateId.New.Name, result.State.Name);
        }

        [Fact]
        public async Task ShouldReturnUnspecifiedResponseWhenComponentIsNull()
        {
            // Arrange
            this.component.Add(null);

            // Act
            var result = await this.component.Apply(new TestAction("TEST"), TestStateId.New);

            // Assert
            Assert.Equal(StateMachineResponse.Unspecified.Id, result.Response.Id);
            Assert.Equal(TestStateId.New.Name, result.State.Name);
        }

        [Fact]
        public async Task ShouldReturnSuccessResponse()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            this.component.Add(this.next);

            Mock.Get(this.next)
                .Setup(n => n.Apply(input, state))
                .ReturnsAsync(new StateTransitionResponse(StateMachineResponse.Success, TestStateId.Processing));

            // Act
            var result = await this.component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Success.Id, result.Response.Id);
            Assert.Equal(TestStateId.Processing.Name, result.State.Name);
        }
    }
}
