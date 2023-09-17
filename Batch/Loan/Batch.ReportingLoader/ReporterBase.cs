using BrassLoon.DataClient;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public abstract class ReporterBase : IDisposable
    {
        private readonly IDbProviderFactory _providerFactory;
        private readonly ISettingsFactory _settingsFactory;
        private bool _disposedValue;
        private DbConnection _connection;

        protected ReporterBase(ISettingsFactory settings, IDbProviderFactory providerFactory)
        {
            _settingsFactory = settings;
            _providerFactory = providerFactory;
        }

        protected ISettingsFactory SettingsFactory => _settingsFactory;

        protected async Task<DbConnection> GetConnection()
        {
            if (_connection == null)
            {
                _connection = await _providerFactory.OpenConnection(_settingsFactory.CreateData());
            }
            return _connection;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_connection != null)
                        _connection.Dispose();
                }

                _connection = null;
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
