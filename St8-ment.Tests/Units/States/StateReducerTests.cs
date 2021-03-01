using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using St8_ment.States;
using St8_ment.Tests.Units.Utilities;
using Xunit;

namespace St8_ment.Tests.Units.States
{
    public class StateReducerTests
    {
        private readonly IStateReducerCore<TestContext> core;
        private readonly IStateReducer<TestContext> reducer;

        public StateReducerTests()
        {
            this.core = Mock.Of<IStateReducerCore<TestContext>>(MockBehavior.Strict);
            this.reducer = new StateReducer<TestContext>(this.core);
        }

        [Fact]
        public void TryGetProviderShouldReturnNullWhenStateIdIsNotInCollection()
        {
            // Arrange
            Mock.Get(this.core)
                .Setup(c => c.TryGet(TestStateId.New, out It.Ref<IActionProvider<TestContext>>.IsAny))
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
                .Setup(c => c.TryGet(TestStateId.New, out It.Ref<IActionProvider<TestContext>>.IsAny))
                .Callback(new StateOutputCallback<TestContext>((StateId _, out IActionProvider<TestContext> provider) =>
                {
                    provider = Mock.Of<IActionProvider<TestContext>>();
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
            var context = new TestContext();

            // Act
            this.reducer.SetState(TestStateId.New, context);

            // Assert
            Assert.NotNull(context.State);
            Assert.Equal(TestStateId.New, context.State.StateId);
            Assert.Equal(context, context.State.Context);
        }
    }
}
