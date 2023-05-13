namespace JestersCreditUnion.Framework
{
    public interface ILoanFactory
    {
        IAddressFactory AddressFactory { get; set; }
        IEmailAddressFactory EmailAddressFactory { get; set; }
        IPhoneFactory PhoneFactory { get; set; }

        ILoan Create(ILoanApplication loanApplication);
    }
}
