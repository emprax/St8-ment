using System.Threading.Tasks;
using Moq;
using St8_ment.StateMachines;
using St8_ment.StateMachines.Components;
using St8_ment.Tests.Units.States;
using Xunit;

namespace St8_ment.Tests.Units.StateMachines
{
    public class ResultComponentTests
    {
        private readonly IStateComponent next;
        private readonly StateId state;
        private readonly IItemStateComponent component;

        public ResultComponentTests()
        {
            this.next = Mock.Of<IStateComponent>(MockBehavior.Strict);
            this.state = TestStateId.Complete;
            this.component = new ResultComponent(this.state);
        }

        [Fact]
        public async Task ShouldReturnSuccessWithoutCallingComponentWhenComponentIsNull()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.Processing;

            // Act
            var result = await this.component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Success.Id, result.Response.Id);
            Assert.Equal(this.state, result.State);
        }

         [Fact]
        public async Task ShouldReturnSuccessWhenCallingComponentWhenComponentIsNull()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.Processing;

            this.component.Add(this.next);

            Mock.Get(this.next)
                .Setup(n => n.Apply(input, this.state))
                .ReturnsAsync(new StateTransitionResponse(StateMachineResponse.Success, this.state));

            // Act
            var result = await this.component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Success.Id, result.Response.Id);
            Assert.Equal(this.state, result.State);
        }
    }
}
