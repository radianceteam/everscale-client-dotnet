using System;
using System.Threading;
using System.Threading.Tasks;

namespace TonSdk.Tests
{
    public class Mutex<T>
    {
        internal T Instance;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public Mutex(T instance)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public async Task<Lock<T>> LockAsync()
        {
            await _semaphoreSlim.WaitAsync();
            return new Lock<T>(this, _semaphoreSlim);
        }
    }

    public class Lock<T> : IDisposable
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly Mutex<T> _mutex;
        private bool _disposed;

        public T Instance
        {
            get
            {
                CheckNotDisposed();
                return _mutex.Instance;
            }
            set
            {
                CheckNotDisposed();
                _mutex.Instance = value;
            }
        }

        public Lock(Mutex<T> mutex, SemaphoreSlim semaphoreSlim)
        {
            _mutex = mutex;
            _semaphoreSlim = semaphoreSlim;
        }

        public void Dispose()
        {
            _semaphoreSlim.Release();
            _disposed = true;
        }

        public T Swap(T newValue)
        {
            CheckNotDisposed();
            var oldValue = _mutex.Instance;
            _mutex.Instance = newValue;
            return oldValue;
        }

        private void CheckNotDisposed()
        {
            if (_disposed)
            {
                throw new InvalidOperationException("Lock already disposed");
            }
        }
    }
}
