using System.Threading.Tasks;
using Moq;
using St8Ment.States;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.States
{
    public class StateTests
    {
        private readonly IStateReducer<TestExtendedStateSubject> reducer;
        private readonly IActionProvider<TestExtendedStateSubject> provider;
        private readonly IActionHandler<TestAction, TestExtendedStateSubject> handler;
        private readonly TestExtendedStateSubject context;

        private readonly IState<TestExtendedStateSubject> state;

        public StateTests()
        {
            this.reducer = Mock.Of<IStateReducer<TestExtendedStateSubject>>(MockBehavior.Strict);
            this.provider = Mock.Of<IActionProvider<TestExtendedStateSubject>>(MockBehavior.Strict);
            this.handler = Mock.Of<IActionHandler<TestAction, TestExtendedStateSubject>>(MockBehavior.Strict);

            this.context = new TestExtendedStateSubject(TestStateId.New);
            this.state = new State<TestExtendedStateSubject>(TestStateId.New, this.context, this.reducer);
        }

        [Fact]
        public async Task ApplyShouldReturnStateActionsNotFoundResponseWhenReducerReturnsFalse()
        {
            // Arrange
            var action = new TestAction(nameof(ApplyShouldReturnStateActionsNotFoundResponseWhenReducerReturnsFalse));

            Mock.Get(this.reducer)
                .Setup(r => r.TryGetProvider(TestStateId.New, out It.Ref<IActionProvider<TestExtendedStateSubject>>.IsAny))
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
                .Setup(r => r.TryGetProvider(TestStateId.New, out It.Ref<IActionProvider<TestExtendedStateSubject>>.IsAny))
                .Callback(new StateOutputCallback<TestExtendedStateSubject>((StateId id, out IActionProvider<TestExtendedStateSubject> provider) =>
                {
                    provider = this.provider;
                }))
                .Returns(true);

            Mock.Get(this.provider)
                .Setup(p => p.TryGet(out It.Ref<IActionHandler<TestAction, TestExtendedStateSubject>>.IsAny))
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
            var action = new TestAction(nameof(ApplyShouldReturnSuccessResponseWhenActionHandlerWasSuccessfullyProcessed));

            Mock.Get(this.reducer)
                .Setup(r => r.TryGetProvider(TestStateId.New, out It.Ref<IActionProvider<TestExtendedStateSubject>>.IsAny))
                .Callback(new StateOutputCallback<TestExtendedStateSubject>((StateId id, out IActionProvider<TestExtendedStateSubject> provider) =>
                {
                    provider = this.provider;
                }))
                .Returns(true);

            Mock.Get(this.provider)
                .Setup(p => p.TryGet(out It.Ref<IActionHandler<TestAction, TestExtendedStateSubject>>.IsAny))
                .Callback(new ActionOutputCallback<TestAction, TestExtendedStateSubject>((out IActionHandler<TestAction, TestExtendedStateSubject> handler) =>
                { 
                    handler = this.handler;
                }))
                .Returns(true);

            Mock.Get(this.handler)
                .Setup(h => h.Execute(action, It.Is<IStateHandle<TestExtendedStateSubject>>(
                    x => x.Subject.StateId.Name.Equals(TestStateId.New.Name) && x.Subject == this.context)))
                .Returns(Task.CompletedTask);

            Mock.Get(this.reducer)
                .Setup(r => r.SetState(It.Is<StateId>(x => x.Name == TestStateId.Processing.Name), this.context));

            // Act
            var result = await this.state.Apply(action);

            // Assert
            Assert.Equal(StateResponse.Success.Id, result?.Id);
        }
    }
}
