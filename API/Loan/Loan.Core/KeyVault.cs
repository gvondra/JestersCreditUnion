﻿using Azure;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using JestersCreditUnion.Loan.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class KeyVault : IKeyVault
    {
        private static readonly SortedSet<string> _knownKeys = new SortedSet<string>();

        public async Task<KeyVaultKey> CreateKey(ISettings settings, string name)
        {
            KeyClient keyClient = new KeyClient(new Uri(settings.EncryptionKeyVault), AzureCredential.DefaultAzureCredential);
            CreateRsaKeyOptions createRsaKeyOptions = new CreateRsaKeyOptions(name);
            createRsaKeyOptions.KeyOperations.Clear();
            createRsaKeyOptions.KeyOperations.Add(KeyOperation.Encrypt);
            createRsaKeyOptions.KeyOperations.Add(KeyOperation.Decrypt);
            Response<KeyVaultKey> response = await keyClient.CreateRsaKeyAsync(createRsaKeyOptions);
            return response.Value;
        }

        public async Task<KeyVaultKey> GetKey(ISettings settings, string name)
        {
            KeyClient keyClient = new KeyClient(new Uri(settings.EncryptionKeyVault), AzureCredential.DefaultAzureCredential);
            Response<KeyVaultKey> response = await keyClient.GetKeyAsync(name);
            return response.Value;
        }

        private static async Task<ICollection<string>> GetKeys(ISettings settings)
        {
            SortedSet<string> keys = new SortedSet<string>();
            KeyClient keyClient = new KeyClient(new Uri(settings.EncryptionKeyVault), AzureCredential.DefaultAzureCredential);
            await foreach (Page<KeyProperties> page in keyClient.GetPropertiesOfKeysAsync().AsPages())
            {
                foreach (KeyProperties properties in page.Values)
                {
                    keys.Add(properties.Name);
                }
            }
            return keys;
        }

        public async Task<bool> KeyExists(ISettings settings, string name)
        {
            bool exists = _knownKeys.Contains(name);
            if (!exists)
            {
                ICollection<string> keys = await GetKeys(settings);
                exists = keys.Any(k => string.Equals(name, k, StringComparison.OrdinalIgnoreCase));
                if (exists)
                {
                    lock (_knownKeys)
                    {
                        _knownKeys.Add(name);
                    }
                }
            }
            return exists;
        }

        public async Task<byte[]> Encrypt(ISettings settings, string name, byte[] value)
        {
            KeyClient keyClient = new KeyClient(new Uri(settings.EncryptionKeyVault), AzureCredential.DefaultAzureCredential);
            CryptographyClient cryptographyClient = keyClient.GetCryptographyClient(name);
            EncryptResult result = await cryptographyClient.EncryptAsync(EncryptionAlgorithm.RsaOaep256, value);
            return result.Ciphertext;
        }

        public async Task<byte[]> Decrypt(ISettings settings, string name, byte[] value)
        {
            KeyClient keyClient = new KeyClient(new Uri(settings.EncryptionKeyVault), AzureCredential.DefaultAzureCredential);
            CryptographyClient cryptographyClient = keyClient.GetCryptographyClient(name);
            DecryptResult result = await cryptographyClient.DecryptAsync(EncryptionAlgorithm.RsaOaep256, value);
            return result.Plaintext;
        }
    }
}
