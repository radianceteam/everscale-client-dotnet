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
            _outputHelper.WriteLine($"[DEBUG] {message}");
        }

        public void Information(string message)
        {
            _outputHelper.WriteLine($"[INFO] {message}");
        }

        public void Warning(string message)
        {
            _outputHelper.WriteLine($"[WARN] {message}");
        }

        public void Error(string message, Exception ex = null)
        {
            _outputHelper.WriteLine(ex == null ?
                $"[ERROR] {message}" :
                $"[ERROR] {message}: {ex}");
        }
    }
}
