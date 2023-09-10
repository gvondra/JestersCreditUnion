#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace JestersCreditUnion.Loan.Framework.Enumerations
{
    public enum PaymentStatus : short
    {
        Unprocessed = 0,
        Processed = 1
    }

    public enum LoanStatus : short
    {
        New = 0,
        Open = 1,
        Closed = 2
    }

    public enum LoanApplicationStatus : short
    {
        Unset = 0,
        UnderReview = 1,
        Denied = 2,
        Approved = 3,
        NoDecision = 4
    }

    public enum LoanAgrementStatus : short
    {
        Unset = 0,
        New = 1,
        PendingSignoff = 2,
        Agreed = 3,
        Rejected = 4,
        Abandonded = 5
    }

    public enum WorkTaskContextReferenceType : short
    {
        Unset = 0,
        LoanApplicationId = 1,
        LoanId = 2
    }

    public enum LoanApplicationDenialReason : short
    {
        Other = 0,
        Unemployed = 1,
        ExistingDebt = 2,
        Income = 3,
        Residency = 4
    }

    public enum LoanPaymentFrequency : short
    {
        Annual = 1,
        Semiannual = 2,
        Monthly = 12,
        Semimonthly = 24,
        Fortnightly = 26
    }

    public enum TransactionType : short
    {
        NotSet = 0,
        Disbursement = 1,
        PrincipalPayment = 2,
        InterestPayment = 3
    }
}
#pragma warning restore IDE0130 // Namespace does not match folder structure
