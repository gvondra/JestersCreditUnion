using JestersCreditUnion.Framework;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    internal class IdentificationCardReader : IIdentificationCardReader
    {
        private readonly LoanApplication _loanApplication;

        internal IdentificationCardReader(LoanApplication loanApplication)
        {
            _loanApplication = loanApplication;
        }

        public IBlob Blob { get; set; } = new Blob();
        public IKeyVault KeyVault { get; set; } = new KeyVault();

        public async Task<Stream> ReadBorrowerIdentificationCard(ISettings settings)
        {
            if (string.IsNullOrEmpty(settings.EncryptionKeyVault))
                throw new ArgumentException($"Cannot read identifiation card. {nameof(ISettings.EncryptionKeyVault)} is not set");
            if (string.IsNullOrEmpty(settings.IdentitificationCardContainerName))
                throw new ArgumentException($"Cannot read identifiation card. {nameof(ISettings.IdentitificationCardContainerName)} is not set");
            Stream result;
            IdentificationCard card = _loanApplication.BorrowerIdentificationCard;
            if (card == null)
            {
                result = new MemoryStream(Array.Empty<byte>());
            }
            else
            {
                using Aes aes = Aes.Create();
                aes.Mode = CipherMode.CBC;
                aes.KeySize = 256;
                aes.IV = card.InitializationVector;
                aes.Key = await KeyVault.Decrypt(settings, card.MasterKeyName, card.Key);
                using ICryptoTransform transform = aes.CreateDecryptor();
                using CryptoStream cryptoStream = new CryptoStream(
                    await Blob.Download(settings, card.IdentificationCardId.ToString("N")),
                    transform,
                    CryptoStreamMode.Read,
                    false);
                result = new MemoryStream();
                await cryptoStream.CopyToAsync(result);
                result.Position = 0;
            }
            return result;
        }
    }
}
