using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using St8Ment.DependencyInjection.States;
using St8Ment.DependencyInjection.States.Forge;
using St8Ment.States;
using St8Ment.States.Forge;
using St8Ment.Tests.Integration.Utilities;
using Xunit;

namespace St8Ment.Tests.Integration.StateForge
{
    public class StateForgeIntegrationTests
    {
        private readonly IServiceProvider provider;

        public StateForgeIntegrationTests()
        {
            this.provider = new ServiceCollection()
                .AddSingleton<ILogger<TesTSubject>, LoggerMock<TesTSubject>>()
                .AddStateForge((b, _) =>
                {
                    b.Connect<TesTSubject>(coreBuilder =>
                    {
                        coreBuilder
                            .For(TestStateId.Fault)
                            .For(new NewStateForgeStateConfiguration())
                            .For(TestStateId.Processing, builder =>
                            {
                                builder.On<Test3Action>().Handle<Test3ActionHandler>();
                                builder.On<Test2Action>().Handle<Test2ActionHandler>();
                            })
                            .For(TestStateId.Complete, builder =>
                            {
                                builder.On<Test1Action>().Handle<Test1ActionHandler>();
                                builder.On<Test2Action>().Handle<Test2ActionHandler>();
                            });
                    });
                })
                .BuildServiceProvider();
        }

        public class NewStateForgeStateConfiguration : IStateForgeStateConfiguration<TesTSubject>
        {
            public StateId StateId => TestStateId.New;

            public void Configure(IStateForgeStateBuilder<TesTSubject> builder)
            {
                builder.On<Test1Action>().Handle<Test1ActionHandler>();
                builder.On<Test2Action>().Handle<Test2ActionHandler>();
            }
        }

        [Fact]
        public async Task ShouldApplyTest1ActionAndResultInStateProcessing()
        {
            // Arrange
            var context = new TesTSubject(TestStateId.New);
            var forge = this.provider.GetRequiredService<IStateForge>();
            
            // Act
            var result = await forge
                .Connect(context)
                .Apply(new Test1Action());

            // Assert
            Assert.Equal(StateResponse.Success.Id, result.Id);
            Assert.Equal(StateResponse.Success.Name, result.Name);
            Assert.Equal(TestStateId.Processing.Name, context.StateId.Name);
        }

        [Fact]
        public async Task ShouldReturnNoMatchingActionResultWhenActionIsNotAvailableForState()
        {
            // Arrange
            var context = new TesTSubject(TestStateId.New);
            var forge = this.provider.GetRequiredService<IStateForge>();
            
            // Act
            var result1 = await forge
                .Connect(context)
                .Apply(new Test1Action());

            var result2 = await forge
                .Connect(context)
                .Apply(new Test1Action());

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
            var context = new TesTSubject(TestStateId.New);
            var forge = this.provider.GetRequiredService<IStateForge>();
            
            // Act & Assert
            var result1 = await forge
                .Connect(context)
                .Apply(new Test1Action());

            Assert.Equal(TestStateId.Processing.Name, context.StateId.Name);

            var result2 = await forge
                .Connect(context)
                .Apply(new Test3Action());

            Assert.Equal(TestStateId.Complete.Name, context.StateId.Name);

            var result3 = await forge
                .Connect(context)
                .Apply(new Test2Action());

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
