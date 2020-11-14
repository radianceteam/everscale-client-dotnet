using System;
using Serilog;
using ILogger = TonSdk.ILogger;

namespace TonClientDemo
{
    internal class DemoLogger : ILogger
    {
        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Information(string message)
        {
            Log.Information(message);
        }

        public void Warning(string message)
        {
            Log.Warning(message);
        }

        public void Error(string message, Exception ex = null)
        {
            Log.Error(ex, message);
        }
    }
}
