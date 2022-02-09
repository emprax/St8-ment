using Moq;
using St8Ment.States;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.States
{
    public class StateReducerTests
    {
        private readonly IStateReducerCore<TestExtendedStateSubject> core;
        private readonly IStateReducer<TestExtendedStateSubject> reducer;

        public StateReducerTests()
        {
            this.core = Mock.Of<IStateReducerCore<TestExtendedStateSubject>>(MockBehavior.Strict);
            this.reducer = new StateReducer<TestExtendedStateSubject>(this.core);
        }

        [Fact]
        public void TryGetProviderShouldReturnNullWhenStateIdIsNotInCollection()
        {
            // Arrange
            Mock.Get(this.core)
                .Setup(c => c.TryGet(TestStateId.New, out It.Ref<IActionProvider<TestExtendedStateSubject>>.IsAny))
                .Returns(false);

            // Act
            this.reducer.TryGetProvider(TestStateId.New, out var provider);

            // Assert
            Assert.Null(provider);
        }

        [Fact]
        public void TryGetProviderShouldReturnProvider()
        {
            // Arrange
            Mock.Get(this.core)
                .Setup(c => c.TryGet(TestStateId.New, out It.Ref<IActionProvider<TestExtendedStateSubject>>.IsAny))
                .Callback(new StateOutputCallback<TestExtendedStateSubject>((StateId _, out IActionProvider<TestExtendedStateSubject> provider) =>
                {
                    provider = Mock.Of<IActionProvider<TestExtendedStateSubject>>();
                }))
                .Returns(true);

            // Act
            this.reducer.TryGetProvider(TestStateId.New, out var provider);

            // Assert
            Assert.NotNull(provider);
        }

        [Fact]
        public void SetStateShouldCreateNewStateObjectWithStateProvidedProperties()
        {
            // Arrange
            var context = new TestExtendedStateSubject();

            // Act
            this.reducer.SetState(TestStateId.New, context);

            // Assert
            Assert.NotNull(context.StateId);
            Assert.Equal(TestStateId.New, context.StateId);
        }
    }
}
