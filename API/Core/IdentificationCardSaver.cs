using JestersCreditUnion.Framework;
using System.Security.Cryptography;
using System.IO;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class IdentificationCardSaver : IIdentificationCardSaver
    {
        private readonly LoanApplication _loanApplication;

        internal IdentificationCardSaver(LoanApplication loanApplication)
        {
            _loanApplication = loanApplication;
        }

        public IBlob Blob { get; set; } = new Blob();
        public IKeyVault KeyVault { get; set; } = new KeyVault();

        public async Task SaveBorrowerIdentificationCard(ISettings settings, Stream stream)
        {
            const string keyName = "jcu-borrower-id-card";
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
            await CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), (th) => _loanApplication.Update(th));
        }
    }
}
