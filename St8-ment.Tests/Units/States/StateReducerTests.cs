using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using St8Ment.States;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.States
{
    public class StateReducerTests
    {
        private readonly IStateReducerCore<TestStateSubject> core;
        private readonly IStateReducer<TestStateSubject> reducer;

        public StateReducerTests()
        {
            this.core = Mock.Of<IStateReducerCore<TestStateSubject>>(MockBehavior.Strict);
            this.reducer = new StateReducer<TestStateSubject>(this.core);
        }

        [Fact]
        public void TryGetProviderShouldReturnNullWhenStateIdIsNotInCollection()
        {
            // Arrange
            Mock.Get(this.core)
                .Setup(c => c.TryGet(TestStateId.New, out It.Ref<IActionProvider<TestStateSubject>>.IsAny))
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
                .Setup(c => c.TryGet(TestStateId.New, out It.Ref<IActionProvider<TestStateSubject>>.IsAny))
                .Callback(new StateOutputCallback<TestStateSubject>((StateId _, out IActionProvider<TestStateSubject> provider) =>
                {
                    provider = Mock.Of<IActionProvider<TestStateSubject>>();
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
            var context = new TestStateSubject();

            // Act
            this.reducer.SetState(TestStateId.New, context);

            // Assert
            Assert.NotNull(context.State);
            Assert.Equal(TestStateId.New, context.State.StateId);
            Assert.Equal(context, context.State.Subject);
        }
    }
}
