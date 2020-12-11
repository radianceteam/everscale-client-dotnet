using System;
using System.Threading;
using System.Threading.Tasks;

namespace TonSdk.Tests
{
    internal class Mutex<T>
    {
        private readonly T _instance;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public Mutex(T instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public async Task<Lock<T>> LockAsync()
        {
            await _semaphoreSlim.WaitAsync();
            return new Lock<T>(_instance, _semaphoreSlim);
        }
    }

    internal class Lock<T> : IDisposable
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly T _instance;
        private bool _disposed;

        public T Instance
        {
            get
            {
                if (_disposed)
                {
                    throw new InvalidOperationException("Lock already disposed");
                }
                return _instance;
            }
        }

        public Lock(T instance, SemaphoreSlim semaphoreSlim)
        {
            _instance = instance;
            _semaphoreSlim = semaphoreSlim;
        }

        public void Dispose()
        {
            _semaphoreSlim.Release();
            _disposed = true;
        }
    }
}
