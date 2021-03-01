﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using St8_ment.DependencyInjection.StateMachines;
using St8_ment.StateMachines;
using St8_ment.Tests.Integration.Utilities;
using Xunit;

namespace St8_ment.Tests.Integration.StateMachines
{
    public class StateMachineIntegrationTests
    {
        private readonly IServiceProvider provider;
        private readonly LoggerMock<TestContext> logger;

        public StateMachineIntegrationTests()
        {
            this.logger = new LoggerMock<TestContext>();

            this.provider = new ServiceCollection()
                .AddTransient<Test2Callback>()
                .AddTransient<ILogger<TestContext>>(_ => this.logger)
                .AddStateMachine((builder, p) =>
                {
                    builder
                        .ForInitial(TestStateId.New, bldr =>
                        {
                            bldr.On<Test1Action>().To(TestStateId.Processing)
                                .On<Test2Action>().WithCallback(p.GetRequiredService<Test2Callback>()).To(TestStateId.Complete)
                                .OnDefault().To(TestStateId.Fault);
                        })
                        .For(TestStateId.Processing, bldr =>
                        {
                            bldr.On<Test3Action>().To(TestStateId.New)
                                .On<Test2Action>().WithGuard(action => action.ActionName == "TEST-2").To(TestStateId.Complete)
                                .OnDefault().To(TestStateId.Fault);
                        });
                })
                .BuildServiceProvider();
        }

        [Fact]
        public async Task ShouldMakeRoundTripAndFail()
        {
            // Arrange
            var stateMachine = this.provider.GetRequiredService<IStateMachine>();

            // Act & Assert
            var result1 = await stateMachine.Apply(new Test1Action());
            Assert.Equal(TestStateId.Processing.Name, stateMachine.Current.Name);
            Assert.True(result1.Succeeded);

            var result2 = await stateMachine.Apply(new Test3Action());
            Assert.Equal(TestStateId.New.Name, stateMachine.Current.Name);
            Assert.True(result2.Succeeded);

            var result3 = await stateMachine.Apply("hello");
            Assert.Equal(TestStateId.Fault.Name, stateMachine.Current.Name);
            Assert.True(result3.Succeeded);

            this.logger.VerifyNever();
        }

        [Fact]
        public async Task ShouldCallTransition()
        {
            // Arrange
            var stateMachine = this.provider.GetRequiredService<IStateMachine>();

            // Act & Assert
            var result1 = await stateMachine.Apply(new Test2Action());
            Assert.Equal(TestStateId.Complete.Name, stateMachine.Current.Name);
            Assert.True(result1.Succeeded);

            this.logger.VerifyTimes(LogLevel.Information, 1);
        }

        [Fact]
        public async Task ShouldCallSpecAndPass()
        {
            // Arrange
            var stateMachine = this.provider.GetRequiredService<IStateMachine>();

            // Act & Assert
            var result1 = await stateMachine.Apply(new Test1Action());
            Assert.Equal(TestStateId.Processing.Name, stateMachine.Current.Name);
            Assert.True(result1.Succeeded);

            var result2 = await stateMachine.Apply(new Test2Action());
            Assert.Equal(TestStateId.Complete.Name, stateMachine.Current.Name);
            Assert.True(result2.Succeeded);

            this.logger.VerifyNever();
        }

        [Fact]
        public async Task ShouldCallSpecAndFailSoDefaultIsCalled()
        {
            // Arrange
            var stateMachine = this.provider.GetRequiredService<IStateMachine>();
            var input = new Test2Action
            {
                ActionName = "HELLO"
            };

            // Act & Assert
            var result1 = await stateMachine.Apply(new Test1Action());
            Assert.Equal(TestStateId.Processing.Name, stateMachine.Current.Name);
            Assert.True(result1.Succeeded);

            var result2 = await stateMachine.Apply(input);
            Assert.Equal(TestStateId.Fault.Name, stateMachine.Current.Name);
            Assert.True(result2.Succeeded);

            this.logger.VerifyNever();
        }
    }
}