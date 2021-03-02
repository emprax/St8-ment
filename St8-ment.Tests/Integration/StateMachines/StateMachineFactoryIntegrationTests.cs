using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using St8Ment.DependencyInjection.StateMachines;
using St8Ment.StateMachines;
using St8Ment.Tests.Integration.Utilities;
using Xunit;

namespace St8Ment.Tests.Integration.StateMachines
{
    public class StateMachineFactoryIntegrationTests
    {
        private readonly IServiceProvider provider;
        private readonly LoggerMock<TesTSubject> logger;

        public StateMachineFactoryIntegrationTests()
        {
            this.logger = new LoggerMock<TesTSubject>();

            this.provider = new ServiceCollection()
                .AddTransient<ILogger<TesTSubject>>(_ => this.logger)
                .AddStateMachineFactory<string>((builder, _) =>
                {
                    builder
                        .AddStateMachine("TEST1", buildr => 
                        {
                            buildr
                                .ForInitial(TestStateId.New, bldr =>
                                {
                                    bldr.On<Test1Action>().To(TestStateId.Complete)
                                        .On<Test2Action>().WithCallback<Test2Callback>().To(TestStateId.Processing);
                                })
                                .For(TestStateId.Processing, bldr =>
                                {
                                    bldr.On<Test3Action>().To(TestStateId.New)
                                        .OnDefault().To(TestStateId.Complete);
                                });
                        })
                        .AddStateMachine("TEST2", buildr => 
                        {
                            buildr
                                .ForInitial(TestStateId.New, bldr =>
                                {
                                    bldr.On<Test1Action>().WithCallback<Test1Callback>().To(TestStateId.Complete)
                                        .On<Test2Action>().WithCallback<Test2Callback>().To(TestStateId.Processing);
                                })
                                .For(TestStateId.Processing, bldr =>
                                {
                                    bldr.On<Test3Action>().To(TestStateId.New)
                                        .OnDefault().To(TestStateId.Fault);
                                });
                        });
                })
                .BuildServiceProvider();
        }

        [Fact]
        public async Task ShouldVerifyPathPathOnStateMachineTest1()
        {
            // Arrange
            var factory = this.provider.GetRequiredService<IStateMachineFactory<string>>();
            var stateMacine = factory.Create("TEST1");

            // Act
            await stateMacine.Apply(new Test2Action());
            await stateMacine.Apply(new Test3Action());
            await stateMacine.Apply(new Test1Action());

            // Assert
            this.logger.VerifyTimes(LogLevel.Information, 1);

            Assert.Equal(TestStateId.Complete.Name, stateMacine.Current.Name);
        }

        [Fact]
        public async Task ShouldVerifyPathPathOnStateMachineTest2()
        {
            // Arrange
            var factory = this.provider.GetRequiredService<IStateMachineFactory<string>>();
            var stateMacine = factory.Create("TEST2");

            // Act
            await stateMacine.Apply(new Test2Action());
            await stateMacine.Apply(new Test3Action());
            await stateMacine.Apply(new Test1Action());

            // Assert
            this.logger.VerifyTimes(LogLevel.Information, 2);

            Assert.Equal(TestStateId.Complete.Name, stateMacine.Current.Name);
        }

        [Fact]
        public async Task ShouldVerifyDefaultPathOnStateMachineTest1()
        {
            // Arrange
            var factory = this.provider.GetRequiredService<IStateMachineFactory<string>>();
            var stateMacine = factory.Create("TEST1");

            // Act
            await stateMacine.Apply(new Test2Action());
            await stateMacine.Apply(new Test2Action());

            // Assert
            this.logger.VerifyTimes(LogLevel.Information, 1);

            Assert.Equal(TestStateId.Complete.Name, stateMacine.Current.Name);
        }

        [Fact]
        public async Task ShouldVerifyDefaultPathOnStateMachineTest2()
        {
            // Arrange
            var factory = this.provider.GetRequiredService<IStateMachineFactory<string>>();
            var stateMacine = factory.Create("TEST2");

            // Act
            await stateMacine.Apply(new Test2Action());
            await stateMacine.Apply(new Test2Action());

            // Assert
            this.logger.VerifyTimes(LogLevel.Information, 1);

            Assert.Equal(TestStateId.Fault.Name, stateMacine.Current.Name);
        }
    }
}
