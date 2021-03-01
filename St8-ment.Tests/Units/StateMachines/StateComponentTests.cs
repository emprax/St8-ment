using System.Threading.Tasks;
using Moq;
using St8_ment.StateMachines;
using St8_ment.StateMachines.Components;
using St8_ment.Tests.Units.Utilities;
using Xunit;

namespace St8_ment.Tests.Units.StateMachines
{
    public class StateComponentTests
    {
        private readonly IStateComponent next;
        private readonly IStateComponent defaultComponent;
        private readonly IKeyValueStateComponent<string> component;

        public StateComponentTests()
        {
            this.next = Mock.Of<IStateComponent>(MockBehavior.Strict);
            this.defaultComponent = Mock.Of<IStateComponent>(MockBehavior.Strict);
            this.component = new StateComponent();
        }

        [Fact]
        public async Task ShouldReturnUnspecifiedResponseWhenComponentsAreNotThere()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            // Act
            var result = await this.component.Apply(input, state);

            // Arrange
            Assert.Equal(StateMachineResponse.Unspecified.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnExceptionResponseWhenDefaultComponentReturnsExceptionResponse()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            this.component.Add(typeof(object).FullName, this.defaultComponent);

            Mock.Get(this.defaultComponent)
                .Setup(d => d.Apply(input, state))
                .ReturnsAsync(new StateTransitionResponse(StateMachineResponse.Exception, state));

            // Act
            var result = await this.component.Apply(input, state);

            // Arrange
            Assert.Equal(StateMachineResponse.Exception.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnDefaultResponseWhenDefaultComponentReturnsSuccessResponse()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            this.component.Add(typeof(object).FullName, this.defaultComponent);

            Mock.Get(this.defaultComponent)
                .Setup(d => d.Apply(input, state))
                .ReturnsAsync(new StateTransitionResponse(StateMachineResponse.Success, state));

            // Act
            var result = await this.component.Apply(input, state);

            // Arrange
            Assert.Equal(StateMachineResponse.DefaultTransition.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnDefaultResponseWhenDefaultComponentReturnsSuccessResponseAndComponentReturnsUnsuccessfullResponse()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            this.component.Add(typeof(TestAction).FullName, this.next);
            this.component.Add(typeof(object).FullName, this.defaultComponent);

            Mock.Get(this.next)
                .Setup(n => n.Apply(input, state))
                .ReturnsAsync(new StateTransitionResponse(StateMachineResponse.Exception, state));

            Mock.Get(this.defaultComponent)
                .Setup(d => d.Apply(input, state))
                .ReturnsAsync(new StateTransitionResponse(StateMachineResponse.Success, state));

            // Act
            var result = await this.component.Apply(input, state);

            // Arrange
            Assert.Equal(StateMachineResponse.DefaultTransition.Id, result.Response.Id);
        }

        [Fact]
        public async Task ShouldReturnSuccessResponse()
        {
            // Arrange
            var input = new TestAction("TEST");
            var state = TestStateId.New;

            this.component.Add(typeof(TestAction).FullName, this.next);
            this.component.Add(typeof(object).FullName, this.defaultComponent);

            Mock.Get(this.next)
                .Setup(n => n.Apply(input, state))
                .ReturnsAsync(new StateTransitionResponse(StateMachineResponse.Success, state));

            // Act
            var result = await this.component.Apply(input, state);

            // Arrange
            Assert.Equal(StateMachineResponse.Success.Id, result.Response.Id);

            Assert.True(this.component.TryGetValue(typeof(TestAction).FullName, out var com1));
            Assert.NotNull(com1);

            Assert.True(this.component.TryGetValue(typeof(object).FullName, out var com2));
            Assert.NotNull(com2);
        }
    }
}
