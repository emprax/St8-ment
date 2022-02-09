using System;
using System.Collections.Generic;
using Moq;
using St8Ment.States;
using St8Ment.States.Forge;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.States.Forge
{
    public class StateForgeTests
    {
        private readonly string subjectKey;
        private readonly IStateForge forge;
        private readonly IStateForgeCore core;
        private readonly DependencyProvider provider;
        private readonly TestExtendedStateSubject subject;
        private readonly IDictionary<string, Func<DependencyProvider, IStateForgeCore>> registry;

        public StateForgeTests()
        {
            this.subject = new TestExtendedStateSubject();
            this.core = Mock.Of<IStateForgeCore>(MockBehavior.Strict);
            this.subjectKey = typeof(TestExtendedStateSubject).FullName;
            this.registry = new Dictionary<string, Func<DependencyProvider, IStateForgeCore>>
            {
                [this.subjectKey] = _ => this.core
            };

            this.provider = Mock.Of<DependencyProvider>(MockBehavior.Strict);
            this.forge = new StateForge(this.registry, this.provider);
        }

        [Fact]
        public void ShouldThrowKeyNotFoundExceptionWhenKeyIsNotFound()
        {
            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() => this.forge.Connect(new WrongStateSubject()));

            Assert.Equal($"Could not find state registration for subject of type '{typeof(WrongStateSubject).FullName}'.", exception.Message);
        }

        [Fact]
        public void ShouldCreateStateBySubjectType()
        {
            // Act
            var state = this.forge.Connect(this.subject);

            // Assert
            Assert.NotNull(state);
            Assert.Equal(typeof(TestExtendedStateSubject).FullName, ((St8Ment.States.Forge.State<TestExtendedStateSubject>)state).Subject.GetType().FullName);
        }

        public class WrongStateSubject : StateSubject { }
    }
}
