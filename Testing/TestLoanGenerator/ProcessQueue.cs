using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    internal class ProcessQueue<T> : IDisposable where T : class
    {
        private readonly ConcurrentQueue<T> _innerQueue = new ConcurrentQueue<T>();
        private readonly Thread _generateThread;
        private readonly ManualResetEvent _processExitLock = new ManualResetEvent(false);
        private bool _exit;
        private bool _disposedValue;

        public event EventHandler<IEnumerable<T>> ItemsDequeued;
        
        public ProcessQueue()
        {
            _exit = false;
            _generateThread = new Thread(Process)
            {
                IsBackground = true,
                Name = $"{typeof(T).Name} process queue"
            };
            _generateThread.Start();
        }

        public void Enqueue(T item)
        {
            lock (_innerQueue)
            {
                _innerQueue.Enqueue(item);
                Monitor.PulseAll(_innerQueue);
            }
        }

        private void Process()
        {
            IEnumerable<T> items;
            while (!_exit || _innerQueue.Count > 0)
            {
                items = TryDeque();
                if (items != null && items.Count() > 0)
                    ItemsDequeued.Invoke(this, items);
            }
            _processExitLock.Set();
        }

        private IEnumerable<T> TryDeque()
        {
            List<T> result = new List<T>();
            T item;
            lock (_innerQueue)
            {
                while ((!_exit || _innerQueue.Count > 0) && result.Count == 0)
                {
                    item = null;
                    while (!_exit && !_innerQueue.TryDequeue(out item))
                    {
                        Monitor.Wait(_innerQueue);
                    }
                    if (item != null)
                        result.Add(item);
                    while (_innerQueue.TryDequeue(out item))
                    {
                        result.Add(item);
                    }
                }

            }
            return result;
        }

        public void Shutdown()
        {
            lock (_innerQueue)
            {
                _exit = true;
                Monitor.PulseAll(_innerQueue);
            }            
        }

        public void WaitForProcessExit()
        {
            Shutdown();
            _processExitLock.WaitOne();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Shutdown();
                    try
                    {
                        _generateThread.Join(60000);                        
                    }
                    catch (ThreadStateException) { }
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
