﻿using JestersCreditUnion.Framework;
using System.Security.Cryptography;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Globalization;

namespace JestersCreditUnion.Core
{
    public class IdentificationCardSaver : IIdentificationCardSaver
    {
        private readonly LoanApplication _loanApplication;
        private static readonly object _masterKeyLock = new { };
        private static string _masterKeyName = string.Empty;
        private static DateTime _masterKeyExpiration = DateTime.MinValue;

        internal IdentificationCardSaver(LoanApplication loanApplication)
        {
            _loanApplication = loanApplication;
        }

        public IBlob Blob { get; set; } = new Blob();
        public IKeyVault KeyVault { get; set; } = new KeyVault();

        public async Task SaveBorrowerIdentificationCard(ISettings settings, Stream stream)
        {
            string keyName = GetMasterKey();
            IdentificationCard card = _loanApplication.BorrowerIdentificationCard;
            if (card == null)
                card = _loanApplication.SetBorrowerIdentificationCard(null, null);
            string name = card.IdentificationCardId.ToString("N");
            if (!await KeyVault.KeyExists(settings, keyName))
                await KeyVault.CreateKey(settings, keyName);

            using Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.KeySize = 256;
            using ICryptoTransform transform = aes.CreateEncryptor();
            using CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read, false);
            await Blob.Upload(settings, name, cryptoStream);

            card.InitializationVector = aes.IV;
            card.Key = await KeyVault.Encrypt(settings, keyName, aes.Key);
            card.MasterKeyName = keyName;
            await CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), (th) => _loanApplication.Update(th));
        }

        private static string GetMasterKey()
        {
            if (_masterKeyExpiration < DateTime.Now)
            {
                lock (_masterKeyLock)
                {
                    if (_masterKeyExpiration < DateTime.Now)
                    {
                        _masterKeyName = string.Format(CultureInfo.InvariantCulture, "mk{0:N}", Guid.NewGuid());
                        _masterKeyExpiration = DateTime.Now.AddHours(6);
                    }
                }
            }
            return _masterKeyName;
        }
    }
}
