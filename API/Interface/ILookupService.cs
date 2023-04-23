using JestersCreditUnion.Interface.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface ILookupService
    {
        Task<List<string>> GetIndex(ISettings settings);
        Task<Lookup> Get(ISettings settings, string code);
        Task<Lookup> Save(ISettings settings, string code, Dictionary<string, string> data);
        Task Delete(ISettings settings, string code);
    }
}
