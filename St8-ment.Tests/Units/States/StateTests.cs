using System.Threading.Tasks;
using Moq;
using St8Ment.States;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.States
{
    public class StateTests
    {
        private readonly IStateReducer<TestContext> reducer;
        private readonly IActionProvider<TestContext> provider;
        private readonly IActionHandler<TestAction, TestContext> handler;
        private readonly TestContext context;

        private readonly IState<TestContext> state;

        public StateTests()
        {
            this.reducer = Mock.Of<IStateReducer<TestContext>>(MockBehavior.Strict);
            this.provider = Mock.Of<IActionProvider<TestContext>>(MockBehavior.Strict);
            this.handler = Mock.Of<IActionHandler<TestAction, TestContext>>(MockBehavior.Strict);
            this.context = new TestContext();
            
            this.state = new State<TestContext>(TestStateId.New, this.context, this.reducer);
        }

        [Fact]
        public async Task ApplyShouldReturnStateActionsNotFoundResponseWhenReducerReturnsFalse()
        {
            // Arrange
            var action = new TestAction(nameof(ApplyShouldReturnStateActionsNotFoundResponseWhenReducerReturnsFalse));

            Mock.Get(this.reducer)
                .Setup(r => r.TryGetProvider(TestStateId.New, out It.Ref<IActionProvider<TestContext>>.IsAny))
                .Returns(false);

            // Act
            var result = await this.state.Apply(action);

            // Assert
            Assert.Equal(StateResponse.StateActionsNotFound.Id, result?.Id);
        }

        [Fact]
        public async Task ApplyShouldReturnNoMatchingActionResponseWhenProviderReturnsFalse()
        {
            // Arrange
            var action = new TestAction(nameof(ApplyShouldReturnNoMatchingActionResponseWhenProviderReturnsFalse));

            Mock.Get(this.reducer)
                .Setup(r => r.TryGetProvider(TestStateId.New, out It.Ref<IActionProvider<TestContext>>.IsAny))
                .Callback(new StateOutputCallback<TestContext>((StateId id, out IActionProvider<TestContext> provider) =>
                {
                    provider = this.provider;
                }))
                .Returns(true);

            Mock.Get(this.provider)
                .Setup(p => p.TryGet(out It.Ref<IActionHandler<TestAction, TestContext>>.IsAny))
                .Returns(false);

            // Act
            var result = await this.state.Apply(action);

            // Assert
            Assert.Equal(StateResponse.NoMatchingAction.Id, result?.Id);
        }

        [Fact]
        public async Task ApplyShouldReturnSuccessResponseWhenActionHandlerWasSuccessfullyProcessed()
        {
            // Arrange
            var action = new TestAction(nameof(ApplyShouldReturnNoMatchingActionResponseWhenProviderReturnsFalse));

            Mock.Get(this.reducer)
                .Setup(r => r.TryGetProvider(TestStateId.New, out It.Ref<IActionProvider<TestContext>>.IsAny))
                .Callback(new StateOutputCallback<TestContext>((StateId id, out IActionProvider<TestContext> provider) =>
                {
                    provider = this.provider;
                }))
                .Returns(true);

            Mock.Get(this.provider)
                .Setup(p => p.TryGet(out It.Ref<IActionHandler<TestAction, TestContext>>.IsAny))
                .Callback(new ActionOutputCallback<TestAction, TestContext>((out IActionHandler<TestAction, TestContext> handler) =>
                { 
                    handler = this.handler;
                }))
                .Returns(true);

            Mock.Get(this.handler)
                .Setup(h => h.Execute(action, It.Is<IStateView<TestContext>>(x => x.StateId == TestStateId.New && x.Context == this.context)))
                .ReturnsAsync(TestStateId.Processing);

            Mock.Get(this.reducer)
                .Setup(r => r.SetState(It.Is<StateId>(x => x.Name == TestStateId.Processing.Name), this.context));

            // Act
            var result = await this.state.Apply(action);

            // Assert
            Assert.Equal(StateResponse.Success.Id, result?.Id);
        }
    }
}
