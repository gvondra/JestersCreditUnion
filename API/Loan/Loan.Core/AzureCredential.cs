﻿using Azure.Identity;

namespace JestersCreditUnion.Loan.Core
{
    public sealed class AzureCredential
    {
        public static DefaultAzureCredential DefaultAzureCredential { get; } = CreateDefaultAzureCredential();

        private static DefaultAzureCredential CreateDefaultAzureCredential() => new DefaultAzureCredential(GetDefaultAzureCredentialOptions());

        public static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions()
        {
            return new DefaultAzureCredentialOptions()
            {
                ExcludeAzureCliCredential = false,
                ExcludeAzurePowerShellCredential = false,
                ExcludeSharedTokenCacheCredential = true,
                ExcludeEnvironmentCredential = false,
                ExcludeManagedIdentityCredential = false,
                ExcludeVisualStudioCodeCredential = false,
                ExcludeVisualStudioCredential = false
            };
        }
    }
}
