using System;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ISettings : JestersCreditUnion.CommonCore.ISettings
    {
        string BrassLoonAccountApiBaseAddress { get; }
        string BrassLoonConfigApiBaseAddress { get; }
        string BrassLoonWorkTaskApiBaseAddress { get; }
        string BrassLoonAddressApiBaseAddress { get; }
        Guid? BrassLoonClientId { get; }
        string BrassLoonClientSecret { get; }
        Guid? ConfigDomainId { get; }
        Guid? WorkTaskDomainId { get; }
        Guid? AddressDomainId { get; }
        string WorkTaskConfigurationCode { get; }
        string IdentitificationCardContainerName { get; }
        string EncryptionKeyVault { get; }
        string ServiceBusNamespace { get; }
        string ServiceBusNewLoanAppQueue { get; }
    }
}
