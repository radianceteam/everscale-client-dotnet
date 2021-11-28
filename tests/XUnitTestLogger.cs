using System;
using Xunit.Abstractions;

namespace TonSdk.Tests
{
    public class XUnitTestLogger : ILogger
    {
        private readonly ITestOutputHelper _outputHelper;

        public XUnitTestLogger(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
        }

        public void Debug(string message)
        {
            Log("DEBUG", message);
        }

        public void Information(string message)
        {
            Log("INFO", message);
        }

        public void Warning(string message)
        {
            Log("WARN", message);
        }

        public void Error(string message, Exception ex = null)
        {
            Log("ERROR", ex == null ? message : $"{message}: {ex}");
        }

        private void Log(string level, string message)
        {
            try
            {
                _outputHelper.WriteLine($"[{level}] {message}");
            }
            catch (Exception)
            {
                // Ignore System.InvalidOperationException: There is no currently active test.
            }
        }
    }
}
