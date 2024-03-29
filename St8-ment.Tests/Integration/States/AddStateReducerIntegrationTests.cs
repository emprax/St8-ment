﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using St8Ment.DependencyInjection.States;
using St8Ment.States;
using St8Ment.Tests.Integration.Utilities;
using Xunit;

namespace St8Ment.Tests.Integration.States
{
    public class AddStateReducerIntegrationTests
    {
        private readonly IServiceProvider provider;

        public AddStateReducerIntegrationTests()
        {
            this.provider = new ServiceCollection()
                .AddSingleton<ILogger<TesTSubject>, LoggerMock<TesTSubject>>()
                .AddStateReducer<TesTSubject>((builder, _) => 
                {
                    builder
                        .For(TestStateId.Fault)
                        .For(new NewStateConfiguration())
                        .For(TestStateId.Processing, bldr => 
                        {
                            bldr.On<Test3Action>().Handle<Test3ActionHandler>();
                            bldr.On<Test2Action>().Handle<Test2ActionHandler>();
                        })
                        .For(TestStateId.Complete, bldr =>
                        {
                            bldr.On<Test1Action>().Handle<Test1ActionHandler>();
                            bldr.On<Test2Action>().Handle<Test2ActionHandler>();
                        });
                })
                .BuildServiceProvider();
        }

        public class NewStateConfiguration : IStateConfiguration<TesTSubject>
        {
            public StateId StateId => TestStateId.New;

            public void Configure(IStateBuilder<TesTSubject> builder)
            {
                builder.On<Test1Action>().Handle<Test1ActionHandler>();
                builder.On<Test2Action>().Handle<Test2ActionHandler>();
            }
        }

        [Fact]
        public async Task ShouldApplyTest1ActionAndResultInStateProcessing()
        {
            // Arrange
            var context = new TesTSubject();
            var reducer = this.provider.GetRequiredService<IStateReducer<TesTSubject>>();

            reducer.SetState(TestStateId.New, context);

            // Act
            var result = await context.Apply(new Test1Action());

            // Assert
            Assert.Equal(StateResponse.Success.Id, result.Id);
            Assert.Equal(StateResponse.Success.Name, result.Name);
            Assert.Equal(TestStateId.Processing.Name, context.StateId.Name);
        }

        [Fact]
        public async Task ShouldReturnNoMatchingActionResultWhenActionIsNotAvailableForState()
        {
            // Arrange
            var context = new TesTSubject();
            var reducer = this.provider.GetRequiredService<IStateReducer<TesTSubject>>();

            reducer.SetState(TestStateId.New, context);

            // Act
            var result1 = await context.Apply(new Test1Action());
            var result2 = await context.Apply(new Test1Action());

            // Assert
            Assert.Equal(StateResponse.Success.Id, result1.Id);
            Assert.Equal(StateResponse.Success.Name, result1.Name);

            Assert.Equal(StateResponse.NoMatchingAction.Id, result2.Id);
            Assert.Equal(StateResponse.NoMatchingAction.Name, result2.Name);

            Assert.Equal(TestStateId.Processing.Name, context.StateId.Name);
        }

        [Fact]
        public async Task ShouldTransitionIntoMultipleStates()
        {
            // Arrange
            var context = new TesTSubject();
            var reducer = this.provider.GetRequiredService<IStateReducer<TesTSubject>>();

            reducer.SetState(TestStateId.New, context);

            // Act & Assert
            var result1 = await context.Apply(new Test1Action());
            Assert.Equal(TestStateId.Processing.Name, context.StateId.Name);

            var result2 = await context.Apply(new Test3Action());
            Assert.Equal(TestStateId.Complete.Name, context.StateId.Name);

            var result3 = await context.Apply(new Test2Action());
            Assert.Equal(TestStateId.Fault.Name, context.StateId.Name);

            Assert.Equal(StateResponse.Success.Id, result1.Id);
            Assert.Equal(StateResponse.Success.Name, result1.Name);

            Assert.Equal(StateResponse.Success.Id, result2.Id);
            Assert.Equal(StateResponse.Success.Name, result2.Name);

            Assert.Equal(StateResponse.Success.Id, result3.Id);
            Assert.Equal(StateResponse.Success.Name, result3.Name);
        }
    }
}
