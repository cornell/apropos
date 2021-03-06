﻿using Apropos.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Apropos.Test.Core
{
    public class LoggerStub : ILogger<ArticleService>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Debugger.Break();
        }
    }
}
