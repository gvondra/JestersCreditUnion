namespace JestersCreditUnion.Framework.Enumerations
{
    public enum LoanApplicationStatus : short
    {
        Unset = 0,
        UnderReview = 1,
        Denied = 2,
        Approved = 3,
        NoDecision = 4
    }

    public enum WorkTaskContextReferenceType : short
    {
        Unset = 0,
        LoanApplicationId = 1
    }
}
