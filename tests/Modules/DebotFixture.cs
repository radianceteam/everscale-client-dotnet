using System;
using System.Threading.Tasks;
using TonSdk.Modules;

namespace TonSdk.Tests.Modules
{
    public class DebotFixture<T> where T : class, ITestDebot
    {
        public T Debot => _debot;

        private readonly T _debot;

        public DebotFixture()
        {
            _debot = (T)Activator.CreateInstance(typeof(T));
            if (_debot == null)
            {
                throw new NullReferenceException("Failed to create debot of type " + typeof(T).Name);
            }
        }

        public async Task<TestDebotBrowser> GetDebotBrowserAsync(ILogger logger)
        {
            await _debot.InitAsync(logger);
            return new TestDebotBrowser(_debot, logger);
        }
    }
}
