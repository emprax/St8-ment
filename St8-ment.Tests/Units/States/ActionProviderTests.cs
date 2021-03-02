using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Moq;
using St8Ment.States;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.States
{
    public class ActionProviderTests
    {
        private readonly ConcurrentDictionary<string, Func<object>> handlers;
        private readonly IActionHandler<TestAction, TestStateSubject> handler;
        private readonly IActionProvider<TestStateSubject> actionProvider;

        public class NoneExistingAction : IAction { }

        public class NullAction : IAction { }

        public class DummyAction : IAction { }

        public class OtherAction : IAction { }

        public ActionProviderTests()
        {
            this.handler = Mock.Of<IActionHandler<TestAction, TestStateSubject>>(MockBehavior.Strict);
            this.handlers = new ConcurrentDictionary<string, Func<object>>(new[]
            {
                new KeyValuePair<string, Func<object>>(typeof(TestAction).FullName, () => this.handler),
                new KeyValuePair<string, Func<object>>(typeof(NullAction).FullName, null),
                new KeyValuePair<string, Func<object>>(typeof(DummyAction).FullName, () => null),
                new KeyValuePair<string, Func<object>>(typeof(OtherAction).FullName, () => "hello")
            });

            this.actionProvider = new ActionProvider<TestStateSubject>(this.handlers);
        }

        [Fact]
        public void TryGetShouldReturnFalseWhenActionCannotBeFound() => this.TryGetFailureTestSetup<NoneExistingAction>();

        [Fact]
        public void TryGetShouldReturnFalseWhenActionFactoryIsNull() => this.TryGetFailureTestSetup<NullAction>();

        [Fact]
        public void TryGetShouldReturnFalseWhenActionFactoryReturnsNull() => this.TryGetFailureTestSetup<DummyAction>();

        [Fact]
        public void TryGetShouldReturnFalseWhenActionFactoryReturnsNoneConvertable() => this.TryGetFailureTestSetup<OtherAction>();

        [Fact]
        public void TryGetShouldReturnActionHandler()
        {
            // Act
            var result = this.actionProvider.TryGet<TestAction>(out var actionHandler);

            // Assert
            Assert.True(result);
            Assert.NotNull(actionHandler);
        }

        private void TryGetFailureTestSetup<TAction>() where TAction : class, IAction
        {
            // Act
            var result = this.actionProvider.TryGet<TAction>(out var actionHandler);

            // Assert
            Assert.False(result);
            Assert.Null(actionHandler);
        }
    }
}
