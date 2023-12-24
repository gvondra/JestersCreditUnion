using JestersCreditUnion.Loan.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.LoanPaymentProcessor
{
    public class CoreSettings : ISettings
    {
        private readonly Settings _settings;

        public CoreSettings(Settings settings)
        {
            _settings = settings;
        }

        public string BrassLoonAccountApiBaseAddress => _settings.BrassLoonAccountApiBaseAddress;

        public string BrassLoonConfigApiBaseAddress => _settings.BrassLoonConfigApiBaseAddress;

        public string BrassLoonWorkTaskApiBaseAddress => _settings.BrassLoonWorkTaskApiBaseAddress;

        public Guid? BrassLoonClientId => _settings.BrassLoonLogClientId;

        public string BrassLoonClientSecret => _settings.BrassLoonLogClientSecret;

        public Guid? ConfigDomainId => _settings.ConfigDomainId;

        public Guid? WorkTaskDomainId => _settings.WorkTaskDomainId;

        public string WorkTaskConfigurationCode => _settings.WorkTaskConfigurationCode;

        public bool UseDefaultAzureSqlToken => _settings.UseDefaultAzureSqlToken;

        public string IdentitificationCardContainerName => throw new NotImplementedException();

        public string EncryptionKeyVault => throw new NotImplementedException();

        public string BrassLoonAddressApiBaseAddress => _settings.BrassLoonAddressApiBaseAddress;

        public Guid? AddressDomainId => _settings.AddressDomainId;

        public Task<string> GetConnetionString() => Task.FromResult(_settings.ConnectionString);

        public Func<Task<string>> GetDatabaseAccessToken() => null;
    }
}
