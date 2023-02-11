using JestersCreditUnion.Data.Models;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface IPhoneDataSaver
    {
        Task Create(IDataSettings settings, PhoneData data);
    }
}
