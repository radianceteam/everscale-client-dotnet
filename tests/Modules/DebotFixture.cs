using System;
using System.Threading.Tasks;

namespace TonSdk.Tests.Modules
{
    public class DebotFixture<T> where T : ITestDebot
    {
        public T Debot => _debot;

        private T _debot;

        public async Task<TestDebotBrowser> GetDebotBrowserAsync(ILogger logger)
        {
            if (_debot == null)
            {
                _debot = (T)Activator.CreateInstance(typeof(T));
                if (_debot == null)
                {
                    throw new NullReferenceException("Failed to create debot of type " + typeof(T).Name);
                }
                await _debot.InitAsync(logger);
            }
            return new TestDebotBrowser(_debot, logger);
        }
    }
}
