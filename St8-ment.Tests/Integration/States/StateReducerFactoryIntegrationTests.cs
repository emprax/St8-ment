using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using St8Ment.DependencyInjection.States;
using St8Ment.States;
using St8Ment.Tests.Integration.Utilities;
using Xunit;

namespace St8Ment.Tests.Integration.States
{
    public class StateReducerFactoryIntegrationTests
    {
        private readonly IServiceProvider provider;
        private readonly LoggerMock<TesTSubject> logger;

        public StateReducerFactoryIntegrationTests()
        {
            this.logger = new LoggerMock<TesTSubject>();

            this.provider = new ServiceCollection()
                .AddTransient<ILogger<TesTSubject>>(_ => this.logger)
                .AddStateReducerFactory<string, TesTSubject>((builder, _) =>
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
                .GetRequiredService<IStateReducerFactory<string, TesTSubject>>()
                .Create("TEST1");

            var context = new TesTSubject();
            reducer.SetState(TestStateId.New, context);

            // Act & Assert
            await context.Apply(new Test1Action());
            var result1 = await context.Apply(new Test2Action());

            Assert.Equal(TestStateId.Processing.Name, context.StateId.Name);
            Assert.Equal(StateResponse.NoMatchingAction.Id, result1.Id);

            reducer.SetState(TestStateId.New, context);

            await context.Apply(new Test1Action());
            var result2 = await context.Apply(new Test3Action());

            Assert.Equal(TestStateId.Complete.Name, context.StateId.Name);
            Assert.Equal(StateResponse.Success.Id, result2.Id);
        }

        [Fact]
        public async Task ShouldVerifyStateReducer2FromFactory()
        {
            // Arrange
            var reducer = this.provider
                .GetRequiredService<IStateReducerFactory<string, TesTSubject>>()
                .Create("TEST2");

            var context = new TesTSubject();
            reducer.SetState(TestStateId.New, context);

            // Act & Assert
            await context.Apply(new Test1Action());
            var result1 = await context.Apply(new Test2Action());

            Assert.Equal(TestStateId.Fault.Name, context.StateId.Name);
            Assert.Equal(StateResponse.Success.Id, result1.Id);

            reducer.SetState(TestStateId.New, context);

            await context.Apply(new Test1Action());
            var result2 = await context.Apply(new Test3Action());

            Assert.Equal(TestStateId.Processing.Name, context.StateId.Name);
            Assert.Equal(StateResponse.NoMatchingAction.Id, result2.Id);
        }
    }
}
