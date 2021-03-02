using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Moq;
using St8Ment.StateMachines;
using St8Ment.StateMachines.Components;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.StateMachines
{
    public class StateMachineFactoryTests
    {
        private readonly IStateMachineCore core;
        private readonly IStateMachineFactory<string> factory;

        public StateMachineFactoryTests()
        {
            this.core = Mock.Of<IStateMachineCore>(MockBehavior.Strict);
            this.factory = new StateMachineFactory<string>(new ConcurrentDictionary<string, Func<IStateMachineCore>>(new[]
            {
                new KeyValuePair<string, Func<IStateMachineCore>>("TEST", () => this.core)
            }));
        }

        [Fact]
        public void ShouldNotCreateStateMachineWhenCoreInstanceCannotByFound()
        {
            // Act & Assert
            Assert.Null(this.factory.Create("NONE"));
        }

        [Fact]
        public void ShouldCreateStateMachine()
        {
            // Arrange
            Mock.Get(this.core)
                .SetupGet(c => c.Component)
                .Returns(Mock.Of<IStateComponent>());

            Mock.Get(this.core)
                .SetupGet(c => c.InitialStateId)
                .Returns(TestStateId.New);

            // Act
            var result = this.factory.Create("TEST");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TestStateId.New.Name, result.Current.Name);
        }
    }
}
