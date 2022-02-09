using System.Threading.Tasks;
using Moq;
using St8Ment.States;
using St8Ment.States.Forge;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.States.Forge
{
    public class StateTests
    {
        private readonly IStateCore core;
        private readonly IStateForgeCore forgeCore;
        private readonly IActionHandler<TestAction, TestStateSubject> handler;
        private readonly TestStateSubject context;

        private readonly IState state;

        public StateTests()
        {
            this.handler = Mock.Of<IActionHandler<TestAction, TestStateSubject>>(MockBehavior.Strict);
            this.forgeCore = Mock.Of<IStateForgeCore>(MockBehavior.Strict);
            this.core = Mock.Of<IStateCore>(MockBehavior.Strict);
            this.context = new TestStateSubject(TestStateId.New);
            
            this.state = new St8Ment.States.Forge.State<TestStateSubject>(this.forgeCore, this.context);
        }

        [Fact]
        public async Task ApplyShouldReturnStateActionsNotFoundResponseWhenStateForgeCoreHasNoResult()
        {
            // Arrange
            var action = new TestAction(nameof(ApplyShouldReturnStateActionsNotFoundResponseWhenStateForgeCoreHasNoResult));

            Mock.Get(this.forgeCore)
                .Setup(r => r.GetForState(TestStateId.New))
                .Returns(default(IStateCore));

            // Act
            var result = await this.state.Apply(action);

            // Assert
            Assert.Equal(StateResponse.StateActionsNotFound.Id, result?.Id);
        }

        [Fact]
        public async Task ApplyShouldReturnNoMatchingActionResponseWhenStateCoreHasNoResult()
        {
            // Arrange
            var action = new TestAction(nameof(ApplyShouldReturnNoMatchingActionResponseWhenStateCoreHasNoResult));

            Mock.Get(this.forgeCore)
                .Setup(r => r.GetForState(TestStateId.New))
                .Returns(this.core);

            Mock.Get(this.core)
                .Setup(p => p.GetHandler<TestAction>())
                .Returns(default(IActionHandler));

            // Act
            var result = await this.state.Apply(action);

            // Assert
            Assert.Equal(StateResponse.NoMatchingAction.Id, result?.Id);
        }

        [Fact]
        public async Task ApplyShouldReturnNoMatchingActionResponseWhenStateCoreHasInvalidConvertedResponse()
        {
            // Arrange
            var action = new TestAction(nameof(ApplyShouldReturnNoMatchingActionResponseWhenStateCoreHasNoResult));
            var wrongMock = Mock.Of<IActionHandler<WrongAction, TestStateSubject>>();

            Mock.Get(this.forgeCore)
                .Setup(r => r.GetForState(TestStateId.New))
                .Returns(this.core);

            Mock.Get(this.core)
                .Setup(p => p.GetHandler<TestAction>())
                .Returns(wrongMock);

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

            Mock.Get(this.forgeCore)
                .Setup(r => r.GetForState(TestStateId.New))
                .Returns(this.core);

            Mock.Get(this.core)
                .Setup(p => p.GetHandler<TestAction>())
                .Returns(this.handler);

            Mock.Get(this.handler)
                .Setup(h => h.Execute(action, It.Is<IStateHandle<TestStateSubject>>(x =>
                    x.Subject.StateId.Name.Equals(TestStateId.New.Name) &&
                    x.Subject == this.context)))
                .Callback<TestAction, IStateHandle<TestStateSubject>>((_, subject) =>
                {
                    subject.Transition(TestStateId.Complete);
                })
                .Returns(Task.CompletedTask);

            // Act
            var result = await this.state.Apply(action);

            // Assert
            Assert.Equal(StateResponse.Success.Id, result?.Id);
            Assert.Equal(TestStateId.Complete, ((St8Ment.States.Forge.State<TestStateSubject>)this.state).Subject.StateId);
        }
    }

    public class WrongAction : IAction { }
}
