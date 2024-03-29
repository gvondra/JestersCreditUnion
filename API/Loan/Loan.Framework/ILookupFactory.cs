﻿using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILookupFactory
    {
        Task<ILookup> GetLookup(ISettings settings, string code);
        Task<ILookup> GetLookup(ISettings settings, Type type);
    }
}
