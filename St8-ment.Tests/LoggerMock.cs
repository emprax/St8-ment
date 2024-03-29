﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Xunit;

namespace St8Ment.Tests
{
    public class LoggerMock<T> : ILogger<T>, IDisposable
    {
        private readonly ConcurrentDictionary<LogLevel, KeyValuePair<uint, string[]>> levels;

        public LoggerMock()
        {
            this.levels = new ConcurrentDictionary<LogLevel, KeyValuePair<uint, string[]>>(new[]
            {
                new KeyValuePair<LogLevel, KeyValuePair<uint, string[]>>(LogLevel.Information, new KeyValuePair<uint, string[]>(0, Array.Empty<string>())),
                new KeyValuePair<LogLevel, KeyValuePair<uint, string[]>>(LogLevel.Error, new KeyValuePair<uint, string[]>(0, Array.Empty<string>())),
                new KeyValuePair<LogLevel, KeyValuePair<uint, string[]>>(LogLevel.Warning, new KeyValuePair<uint, string[]>(0, Array.Empty<string>())),
                new KeyValuePair<LogLevel, KeyValuePair<uint, string[]>>(LogLevel.Critical, new KeyValuePair<uint, string[]>(0, Array.Empty<string>())),
                new KeyValuePair<LogLevel, KeyValuePair<uint, string[]>>(LogLevel.Debug, new KeyValuePair<uint, string[]>(0, Array.Empty<string>())),
                new KeyValuePair<LogLevel, KeyValuePair<uint, string[]>>(LogLevel.Trace, new KeyValuePair<uint, string[]>(0, Array.Empty<string>())),
                new KeyValuePair<LogLevel, KeyValuePair<uint, string[]>>(LogLevel.None, new KeyValuePair<uint, string[]>(0, Array.Empty<string>()))
            });
        }

        public IDisposable BeginScope<TState>(TState state) => this;

        public void Dispose() { }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var (key, value) = this.levels[logLevel];
            var messages = new List<string>(value)
            {
                formatter.Invoke(state, exception)
            };

            this.levels[logLevel] = new KeyValuePair<uint, string[]>(key + 1, messages.ToArray());
        }

        public void VerifyTimes(LogLevel level, uint times) => Assert.Equal(times, this.levels[level].Key);

        public void VerifyNever() => Assert.True(this.levels.All(x => x.Value.Key == 0));

        public void VerifyContains(LogLevel level, string message) => Assert.Contains(message, this.levels[level].Value);

        public void Verify(LogLevel level, uint times, string message)
        {
            this.VerifyTimes(level, times);
            this.VerifyContains(level, message);
        }
    }
}
