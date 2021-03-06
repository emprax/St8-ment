﻿using System;
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
        private readonly ConcurrentDictionary<string, Func<IStateReducerCore<TestStateSubject>>> reducers;
        private readonly IStateReducerCore<TestStateSubject> reducer;
        private readonly IStateReducerFactory<string, TestStateSubject> factory;

        public StateReducerFactoryTests()
        {
            this.reducer = Mock.Of<IStateReducerCore<TestStateSubject>>(MockBehavior.Strict);
            this.reducers = new ConcurrentDictionary<string, Func<IStateReducerCore<TestStateSubject>>>(new[]
            {
                new KeyValuePair<string, Func<IStateReducerCore<TestStateSubject>>>("TEST", () => this.reducer)
            });

            this.factory = new StateReducerFactory<string, TestStateSubject>(this.reducers);
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
