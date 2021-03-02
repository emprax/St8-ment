using System.Threading.Tasks;
using Moq;
using St8Ment.States;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.States
{
    public class StateTests
    {
        private readonly IStateReducer<TestStateSubject> reducer;
        private readonly IActionProvider<TestStateSubject> provider;
        private readonly IActionHandler<TestAction, TestStateSubject> handler;
        private readonly TestStateSubject context;

        private readonly IState<TestStateSubject> state;

        public StateTests()
        {
            this.reducer = Mock.Of<IStateReducer<TestStateSubject>>(MockBehavior.Strict);
            this.provider = Mock.Of<IActionProvider<TestStateSubject>>(MockBehavior.Strict);
            this.handler = Mock.Of<IActionHandler<TestAction, TestStateSubject>>(MockBehavior.Strict);
            this.context = new TestStateSubject();
            
            this.state = new State<TestStateSubject>(TestStateId.New, this.context, this.reducer);
        }

        [Fact]
        public async Task ApplyShouldReturnStateActionsNotFoundResponseWhenReducerReturnsFalse()
        {
            // Arrange
            var action = new TestAction(nameof(ApplyShouldReturnStateActionsNotFoundResponseWhenReducerReturnsFalse));

            Mock.Get(this.reducer)
                .Setup(r => r.TryGetProvider(TestStateId.New, out It.Ref<IActionProvider<TestStateSubject>>.IsAny))
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
                .Setup(r => r.TryGetProvider(TestStateId.New, out It.Ref<IActionProvider<TestStateSubject>>.IsAny))
                .Callback(new StateOutputCallback<TestStateSubject>((StateId id, out IActionProvider<TestStateSubject> provider) =>
                {
                    provider = this.provider;
                }))
                .Returns(true);

            Mock.Get(this.provider)
                .Setup(p => p.TryGet(out It.Ref<IActionHandler<TestAction, TestStateSubject>>.IsAny))
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
                .Setup(r => r.TryGetProvider(TestStateId.New, out It.Ref<IActionProvider<TestStateSubject>>.IsAny))
                .Callback(new StateOutputCallback<TestStateSubject>((StateId id, out IActionProvider<TestStateSubject> provider) =>
                {
                    provider = this.provider;
                }))
                .Returns(true);

            Mock.Get(this.provider)
                .Setup(p => p.TryGet(out It.Ref<IActionHandler<TestAction, TestStateSubject>>.IsAny))
                .Callback(new ActionOutputCallback<TestAction, TestStateSubject>((out IActionHandler<TestAction, TestStateSubject> handler) =>
                { 
                    handler = this.handler;
                }))
                .Returns(true);

            Mock.Get(this.handler)
                .Setup(h => h.Execute(action, It.Is<IStateView<TestStateSubject>>(x => x.StateId == TestStateId.New && x.Subject == this.context)))
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
