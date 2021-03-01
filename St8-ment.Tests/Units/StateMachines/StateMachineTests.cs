using System;
using System.Threading.Tasks;
using Moq;
using St8_ment.StateMachines;
using St8_ment.StateMachines.Components;
using St8_ment.Tests.Units.States;
using Xunit;

namespace St8_ment.Tests.Units.StateMachines
{
    public class StateMachineTests
    {
        private readonly IStateMachineCore core;
        private readonly IStateComponent component;

        public StateMachineTests()
        {
            this.core = Mock.Of<IStateMachineCore>(MockBehavior.Strict);
            this.component = Mock.Of<IStateComponent>(MockBehavior.Strict);
        }

        [Fact]
        public async Task ShouldThrowNotImplementedExceptionWhenCoreComponentIsNull()
        {
            // Arrange
            Mock.Get(this.core)
                .SetupGet(c => c.Component)
                .Returns(default(IStateComponent));

            Mock.Get(this.core)
                .SetupGet(c => c.InitialStateId)
                .Returns(TestStateId.New);

            // Act & Assert
            var exception = await Assert
                .ThrowsAsync<NotImplementedException>(() => new StateMachine(this.core)
                .Apply(new TestAction("TEST")));

            Assert.Equal("The state-machine is not implemented correctly.", exception.Message);
        }

        [Fact]
        public async Task ShouldReturnUnknownStateResponseWhenResultIsNull()
        {
            // Arrange
            var action = new TestAction("TEST");
            var state = TestStateId.New;

            Mock.Get(this.core)
                .SetupGet(c => c.Component)
                .Returns(this.component);

            Mock.Get(this.core)
                .SetupGet(c => c.InitialStateId)
                .Returns(state);

            Mock.Get(this.component)
                .Setup(c => c.Apply(action, state))
                .ReturnsAsync(default(StateTransitionResponse));

            // Act
            var result = await new StateMachine(this.core).Apply(action);

            // Assert
            Assert.Equal(StateMachineResponse.UnknownState.Id, result.Id);
            Assert.Equal($"The provided state identification could not find the corresponding state. Provided state-id: {state.Name}", result.Message);
        }

        [Fact]
        public async Task ShouldReturnUnknownStateResponseWhenResultResponseIsNull()
        {
            // Arrange
            var action = new TestAction("TEST");
            var state = TestStateId.New;
            var resultState = TestStateId.Processing;

            Mock.Get(this.core)
                .SetupGet(c => c.Component)
                .Returns(this.component);

            Mock.Get(this.core)
                .SetupGet(c => c.InitialStateId)
                .Returns(state);

            Mock.Get(this.component)
                .Setup(c => c.Apply(action, state))
                .ReturnsAsync(new StateTransitionResponse(null, resultState));

            // Act
            var result = await new StateMachine(this.core).Apply(action);

            // Assert
            Assert.Equal(TestStateId.Processing.Name, resultState.Name);
            Assert.Equal(StateMachineResponse.UnknownState.Id, result.Id);
            Assert.Equal($"The provided state identification could not find the corresponding state. Provided state-id: {resultState.Name}", result.Message);
        }

        [Fact]
        public async Task ShouldReturnSuccessResponse()
        {
            // Arrange
            var action = new TestAction("TEST");
            var state = TestStateId.New;
            var resultState = TestStateId.Processing;

            Mock.Get(this.core)
                .SetupGet(c => c.Component)
                .Returns(this.component);

            Mock.Get(this.core)
                .SetupGet(c => c.InitialStateId)
                .Returns(state);

            Mock.Get(this.component)
                .Setup(c => c.Apply(action, state))
                .ReturnsAsync(new StateTransitionResponse(StateMachineResponse.Success, resultState));

            // Act
            var result = await new StateMachine(this.core).Apply(action);

            // Assert
            Assert.Equal(TestStateId.Processing.Name, resultState.Name);
            Assert.Equal(StateMachineResponse.Success.Id, result.Id);
            Assert.Equal(StateMachineResponse.Success.Message, result.Message);
        }
    }
}
