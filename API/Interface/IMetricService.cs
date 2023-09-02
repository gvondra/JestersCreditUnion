using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public interface IMetricService
    {
        Task<List<string>> GetEventCodes(ISettings settings);
        Task<List<Metric>> Search(ISettings settings, DateTime maxTimestamp, string eventCode);
    }
}
