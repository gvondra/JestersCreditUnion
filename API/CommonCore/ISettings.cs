﻿using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.CommonCore
{
    public interface ISettings
    {
        string DatabaseHost { get; }
        string DatabaseName { get; }
        string DatabaseUser { get; }
        Task<string> GetDatabasePassword();
    }
}
