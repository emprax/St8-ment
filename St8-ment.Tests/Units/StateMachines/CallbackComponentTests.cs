using System;
using System.Threading.Tasks;
using Moq;
using St8_ment.StateMachines;
using St8_ment.StateMachines.Components;
using St8_ment.Tests.Units.States;
using Xunit;

namespace St8_ment.Tests.Units.StateMachines
{
    public class CallbackComponentTests
    {
        private readonly ITransitionCallback<TestAction> callback;
        private readonly IStateComponent next;

        public CallbackComponentTests()
        {
            this.next = Mock.Of<IStateComponent>(MockBehavior.Strict);
            this.callback = Mock.Of<ITransitionCallback<TestAction>>(MockBehavior.Strict);
        }

        [Fact]
        public async Task ShouldReturnUnspecifiedWhenBothCallbackFactoryAndNextComponentAreNull()
        {
            // Arrange
            var component = new CallbackComponent(null);
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            // Act
            var result = await component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Unspecified.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnUnspecifiedWhenBothCallbackAndNextComponentAreNull()
        {
            // Arrange
            var component = new CallbackComponent(() => null);
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            // Act
            var result = await component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Unspecified.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnUnspecifiedWhenNextComponentIdNullAndCallbackIsNotOfValidType()
        {
            // Arrange
            var component = new CallbackComponent(() => Mock.Of<ITransitionCallback<string>>(MockBehavior.Strict));
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            // Act
            var result = await component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Unspecified.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnExceptionResponseWhenCallbackThrowsException()
        {
            // Arrange
            var component = new CallbackComponent(() => this.callback);
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            Mock.Get(this.callback)
                .Setup(c => c.Execute(input))
                .ThrowsAsync(new InvalidOperationException());

            // Act
            var result = await component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Exception.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnUnspecifiedResponseWhenComponentIsNull()
        {
            // Arrange
            var component = new CallbackComponent(() => this.callback);
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            Mock.Get(this.callback)
                .Setup(c => c.Execute(input))
                .Returns(Task.CompletedTask);

            // Act
            var result = await component.Apply(input, state);

            // Assert
            Assert.Equal(StateMachineResponse.Unspecified.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnSuccess()
        {
            // Arrange
            var component = new CallbackComponent(() => this.callback);
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            component.Add(this.next);

            Mock.Get(this.callback)
                .Setup(c => c.Execute(input))
                .Returns(Task.CompletedTask);

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
