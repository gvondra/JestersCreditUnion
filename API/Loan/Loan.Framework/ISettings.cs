using System;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ISettings : JestersCreditUnion.CommonCore.ISettings
    {
        string BrassLoonAccountApiBaseAddress { get; }
        string BrassLoonConfigApiBaseAddress { get; }
        string BrassLoonWorkTaskApiBaseAddress { get; }
        Guid? BrassLoonClientId { get; }
        string BrassLoonClientSecret { get; }
        Guid? ConfigDomainId { get; }
        Guid? WorkTaskDomainId { get; }
        string WorkTaskConfigurationCode { get; }
        string IdentitificationCardContainerName { get; }
        string EncryptionKeyVault { get; }
    }
}
