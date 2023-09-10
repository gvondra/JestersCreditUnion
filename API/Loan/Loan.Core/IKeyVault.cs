using Azure.Security.KeyVault.Keys;
using JestersCreditUnion.Loan.Framework;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public interface IKeyVault
    {
        Task<KeyVaultKey> CreateKey(ISettings settings, string name);
        Task<KeyVaultKey> GetKey(ISettings settings, string name);
        Task<bool> KeyExists(ISettings settings, string name);
        Task<byte[]> Encrypt(ISettings settings, string name, byte[] value);
        Task<byte[]> Decrypt(ISettings settings, string name, byte[] value);
    }
}
