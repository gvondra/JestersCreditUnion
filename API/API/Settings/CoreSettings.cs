using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using JestersCreditUnion.Framework;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace API
{
    public class CoreSettings : ISettings
    {
        private static Policy _cache = Policy.Cache(new MemoryCacheProvider(new MemoryCache(new MemoryCacheOptions())), new RelativeTtl(TimeSpan.FromMinutes(6)));
        public string KeyVaultAddress { get; set; }
        public string DatabaseHost { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseUser { get; set; }

        public async Task<string> GetDatabasePassword()
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(KeyVaultAddress) && !string.IsNullOrEmpty(DatabaseUser))
            {
                result = await _cache.Execute<Task<string>>(async context =>
                {
                    SecretClientOptions options = new SecretClientOptions()
                    {
                        Retry =
                        {
                            Delay= TimeSpan.FromSeconds(2),
                            MaxDelay = TimeSpan.FromSeconds(16),
                            MaxRetries = 4,
                            Mode = RetryMode.Exponential
                         }
                    };
                    SecretClient client = new SecretClient(
                        new Uri(KeyVaultAddress),
                        new DefaultAzureCredential(
                            new DefaultAzureCredentialOptions()
                            {
                                ExcludeAzureCliCredential = false,
                                ExcludeAzurePowerShellCredential = false,
                                ExcludeSharedTokenCacheCredential = false,
                                ExcludeEnvironmentCredential = false,
                                ExcludeManagedIdentityCredential = false,
                                ExcludeVisualStudioCodeCredential = false,
                                ExcludeVisualStudioCredential = false
                            })
                        , options)
                    ;
                    KeyVaultSecret secret = await client.GetSecretAsync(DatabaseUser);
                    return secret.Value;
                },
                new Context(string.Format("{0} | {1} | {1}", DatabaseHost, DatabaseName, DatabaseUser).ToLower()));
            }
            return result;
        }
    }
}
