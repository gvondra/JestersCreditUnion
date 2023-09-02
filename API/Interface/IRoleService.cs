using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IRoleService
    {
        Task<List<Role>> Get(ISettings settings);
        Task<Role> Create(ISettings settings, Role role);
        Task<Role> Update(ISettings settings, Role role);
        Task<Role> Update(ISettings settings, Guid roleId, Role role);
    }
}
