using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using St8_ment.DependencyInjection.States;
using St8_ment.States;
using St8_ment.Tests.Integration.Utilities;
using Xunit;

namespace St8_ment.Tests.Integration.States
{
    public class AddStateReducerIntegrationTests
    {
        private readonly IServiceProvider provider;

        public AddStateReducerIntegrationTests()
        {
            this.provider = new ServiceCollection()
                .AddSingleton<ILogger<TestContext>, LoggerMock<TestContext>>()
                .AddStateReducer<TestContext>((builder, _) => 
                {
                    builder
                        .For(TestStateId.Fault)
                        .For(TestStateId.New, bldr => 
                        {
                            bldr.On<Test1Action>().Handle<Test1ActionHandler>()
                                .On<Test2Action>().Handle<Test2ActionHandler>();
                        })
                        .For(TestStateId.Processing, bldr => 
                        {
                            bldr.On<Test3Action>().Handle<Test3ActionHandler>()
                                .On<Test2Action>().Handle<Test2ActionHandler>();
                        })
                        .For(TestStateId.Complete, bldr => 
                        {
                            bldr.On<Test1Action>().Handle<Test1ActionHandler>()
                                .On<Test2Action>().Handle<Test2ActionHandler>();
                        });
                })
                .BuildServiceProvider();
        }

        [Fact]
        public async Task ShouldApplyTest1ActionAndResultInStateProcessing()
        {
            // Arrange
            var context = new TestContext();
            var reducer = this.provider.GetRequiredService<IStateReducer<TestContext>>();

            reducer.SetState(TestStateId.New, context);

            // Act
            var result = await context.ApplyAction(new Test1Action());

            // Assert
            Assert.Equal(StateResponse.Success.Id, result.Id);
            Assert.Equal(StateResponse.Success.Name, result.Name);
            Assert.Equal(TestStateId.Processing.Name, context.State.StateId.Name);
        }

        [Fact]
        public async Task ShouldReturnNoMatchingActionResultWhenActionIsNotAvailableForState()
        {
            // Arrange
            var context = new TestContext();
            var reducer = this.provider.GetRequiredService<IStateReducer<TestContext>>();

            reducer.SetState(TestStateId.New, context);

            // Act
            var result1 = await context.ApplyAction(new Test1Action());
            var result2 = await context.ApplyAction(new Test1Action());

            // Assert
            Assert.Equal(StateResponse.Success.Id, result1.Id);
            Assert.Equal(StateResponse.Success.Name, result1.Name);

            Assert.Equal(StateResponse.NoMatchingAction.Id, result2.Id);
            Assert.Equal(StateResponse.NoMatchingAction.Name, result2.Name);

            Assert.Equal(TestStateId.Processing.Name, context.State.StateId.Name);
        }

        [Fact]
        public async Task ShouldTransitionIntoMultipleStates()
        {
            // Arrange
            var context = new TestContext();
            var reducer = this.provider.GetRequiredService<IStateReducer<TestContext>>();

            reducer.SetState(TestStateId.New, context);

            // Act & Assert
            var result1 = await context.ApplyAction(new Test1Action());
            Assert.Equal(TestStateId.Processing.Name, context.State.StateId.Name);

            var result2 = await context.ApplyAction(new Test3Action());
            Assert.Equal(TestStateId.Complete.Name, context.State.StateId.Name);

            var result3 = await context.ApplyAction(new Test2Action());
            Assert.Equal(TestStateId.Fault.Name, context.State.StateId.Name);

            Assert.Equal(StateResponse.Success.Id, result1.Id);
            Assert.Equal(StateResponse.Success.Name, result1.Name);

            Assert.Equal(StateResponse.Success.Id, result2.Id);
            Assert.Equal(StateResponse.Success.Name, result2.Name);

            Assert.Equal(StateResponse.Success.Id, result3.Id);
            Assert.Equal(StateResponse.Success.Name, result3.Name);
        }
    }
}
