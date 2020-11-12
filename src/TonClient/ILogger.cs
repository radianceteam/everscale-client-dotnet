using System;

namespace TonSdk
{
    public interface ILogger
    {
        void Debug(string message);

        void Information(string message);

        void Warning(string message);

        void Error(string message, Exception ex = null);
    }

    public sealed class DummyLogger : ILogger
    {
        private DummyLogger()
        {
        }

        public void Debug(string message)
        {
            // Stub
        }

        public void Information(string message)
        {
            // Stub
        }

        public void Warning(string message)
        {
            // Stub
        }

        public void Error(string message, Exception ex)
        {
            // Stub
        }

        public static readonly ILogger Instance = new DummyLogger();
    }
}
