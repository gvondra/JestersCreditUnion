using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public interface IReporter : IDisposable
    {
        int Order { get; }
        Task PurgeWorkingData();
        Task StageWorkingData();
    }
}
