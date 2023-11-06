using BrassLoon.DataClient;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public abstract class ReporterBase : IDisposable
    {
        private readonly IDbProviderFactory _providerFactory;
        private readonly ISettingsFactory _settingsFactory;
        private bool _disposedValue;
        private DbConnection _sourceConnection;
        private DbConnection _destinationConnection;

        protected ReporterBase(ISettingsFactory settings, IDbProviderFactory providerFactory)
        {
            _settingsFactory = settings;
            _providerFactory = providerFactory;
        }

        protected ISettingsFactory SettingsFactory => _settingsFactory;

        protected async Task<DbConnection> GetSourceConnection()
        {
            if (_sourceConnection == null)
            {
                _sourceConnection = await _providerFactory.OpenConnection(_settingsFactory.CreateSourceData());
            }
            return _sourceConnection;
        }

        protected async Task<DbConnection> GetDestinationConnection()
        {
            if (_destinationConnection == null)
            {
                _destinationConnection = await _providerFactory.OpenConnection(_settingsFactory.CreateDestinationData());
            }
            return _destinationConnection;
        }

        protected static async Task ExecuteNonQuery(DbConnection connection, string procedureName)
        {
            using DbCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = procedureName;
            command.CommandTimeout = 60;
            await command.ExecuteNonQueryAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_sourceConnection != null)
                        _sourceConnection.Dispose();
                    if (_destinationConnection != null)
                        _destinationConnection.Dispose();
                }

                _sourceConnection = null;
                _destinationConnection = null;
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
