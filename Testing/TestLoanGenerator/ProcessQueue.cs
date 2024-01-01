using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    internal class ProcessQueue<T> : IDisposable where T : class
    {
        private const int _maxQueueLength = 16;
        private readonly Queue<T> _innerQueue = new Queue<T>();
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
                while (!_exit && _innerQueue.Count >= _maxQueueLength)
                {
                    Monitor.Wait(_innerQueue);
                }
                bool startedEmpty = _innerQueue.Count == 0;
                _innerQueue.Enqueue(item);
                if (startedEmpty)
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
            lock (_innerQueue)
            {
                while (!_exit && _innerQueue.Count == 0)
                {
                    Monitor.Wait(_innerQueue);
                }
                for (int i = 0; i <= Math.Min(_innerQueue.Count - 1, _maxQueueLength); i += 1)
                {
                    result.Add(_innerQueue.Dequeue());
                }
                if (_innerQueue.Count >= _maxQueueLength - result.Count)
                {
                    Monitor.PulseAll(_innerQueue);
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
