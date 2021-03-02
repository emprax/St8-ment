﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using St8_ment.DependencyInjection.States;
using St8_ment.States;
using St8_ment.Tests.Integration.Utilities;
using Xunit;

namespace St8_ment.Tests.Integration.States
{
    public class StateReducerFactoryIntegrationTests
    {
        private readonly IServiceProvider provider;
        private readonly LoggerMock<TestContext> logger;

        public StateReducerFactoryIntegrationTests()
        {
            this.logger = new LoggerMock<TestContext>();

            this.provider = new ServiceCollection()
                .AddTransient<ILogger<TestContext>>(_ => this.logger)
                .AddStateReducerFactory<string, TestContext>((builder, _) =>
                { 
                    builder
                        .AddStateReducer("TEST1", buildr =>
                        {
                            buildr
                                .For(TestStateId.Complete)
                                .For(TestStateId.New, bldr =>
                                { 
                                    bldr.On<Test1Action>().Handle<Test1ActionHandler>();
                                })
                                .For(TestStateId.Processing, bldr =>
                                { 
                                    bldr.On<Test3Action>().Handle<Test3ActionHandler>();
                                });
                        })
                        .AddStateReducer("TEST2", buildr =>
                        {
                            buildr
                                .For(TestStateId.Fault)
                                .For(TestStateId.New, bldr =>
                                { 
                                    bldr.On<Test1Action>().Handle<Test1ActionHandler>();
                                })
                                .For(TestStateId.Processing, bldr =>
                                { 
                                    bldr.On<Test2Action>().Handle<Test2ActionHandler>();
                                });
                        });
                })
                .BuildServiceProvider();
        }

        [Fact]
        public async Task ShouldVerifyStateReducer1FromFactory()
        {
            // Arrange
            var reducer = this.provider
                .GetRequiredService<IStateReducerFactory<string, TestContext>>()
                .Create("TEST1");

            var context = new TestContext();
            reducer.SetState(TestStateId.New, context);

            // Act & Assert
            await context.ApplyAction(new Test1Action());
            var result1 = await context.ApplyAction(new Test2Action());

            Assert.Equal(TestStateId.Processing.Name, context.State.StateId.Name);
            Assert.Equal(StateResponse.NoMatchingAction.Id, result1.Id);

            reducer.SetState(TestStateId.New, context);

            await context.ApplyAction(new Test1Action());
            var result2 = await context.ApplyAction(new Test3Action());

            Assert.Equal(TestStateId.Complete.Name, context.State.StateId.Name);
            Assert.Equal(StateResponse.Success.Id, result2.Id);
        }

        [Fact]
        public async Task ShouldVerifyStateReducer2FromFactory()
        {
            // Arrange
            var reducer = this.provider
                .GetRequiredService<IStateReducerFactory<string, TestContext>>()
                .Create("TEST2");

            var context = new TestContext();
            reducer.SetState(TestStateId.New, context);

            // Act & Assert
            await context.ApplyAction(new Test1Action());
            var result1 = await context.ApplyAction(new Test2Action());

            Assert.Equal(TestStateId.Fault.Name, context.State.StateId.Name);
            Assert.Equal(StateResponse.Success.Id, result1.Id);

            reducer.SetState(TestStateId.New, context);

            await context.ApplyAction(new Test1Action());
            var result2 = await context.ApplyAction(new Test3Action());

            Assert.Equal(TestStateId.Processing.Name, context.State.StateId.Name);
            Assert.Equal(StateResponse.NoMatchingAction.Id, result2.Id);
        }
    }
}
