using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Moq;
using St8Ment.States;
using St8Ment.Tests.Units.Utilities;
using Xunit;

namespace St8Ment.Tests.Units.States
{
    public class StateReducerFactoryTests
    {
        private readonly ConcurrentDictionary<string, Func<DependencyProvider, IStateReducerCore<TestExtendedStateSubject>>> reducers;
        private readonly IStateReducerCore<TestExtendedStateSubject> reducer;
        private readonly IStateReducerFactory<string, TestExtendedStateSubject> factory;

        public StateReducerFactoryTests()
        {
            this.reducer = Mock.Of<IStateReducerCore<TestExtendedStateSubject>>(MockBehavior.Strict);
            this.reducers = new ConcurrentDictionary<string, Func<DependencyProvider, IStateReducerCore<TestExtendedStateSubject>>>(new[]
            {
                new KeyValuePair<string, Func<DependencyProvider, IStateReducerCore<TestExtendedStateSubject>>>("TEST", _ => this.reducer)
            });

            this.factory = new StateReducerFactory<string, TestExtendedStateSubject>(this.reducers, _ => null);
        }

        [Fact]
        public void CreateShouldReturnNullWhenReducerCoreCannotBeFound()
        {
            // Act & Assert
            Assert.Null(this.factory.Create("NONE"));
        }

        [Fact]
        public void CreateShouldReturnReduder()
        {
            // Act & Assert
            Assert.NotNull(this.factory.Create("TEST"));
        }
    }
}
