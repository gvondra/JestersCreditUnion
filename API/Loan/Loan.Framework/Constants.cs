using JestersCreditUnion.Loan.Framework.Enumerations;
using System.Collections.Generic;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace JestersCreditUnion.Loan.Framework.Constants
{
    public static class WorkTaskConfigurationFields
    {
        public const string NewLoanApplicationTaskTypeCode = "NewLoanApplicationTaskTypeCode";
        public const string SendDenialCorrespondenceTaskTypeCode = "SendDenialCorrespondenceTaskTypeCode";
        public const string DiburseFundsTaskTypeCode = "DiburseFundsTaskTypeCode";
    }

    public static class EnumerationDesriptionLookup
    {
        public static readonly List<(string, Type)> Map = new List<(string, Type)>
        {
            ("loan-application-status", typeof(LoanApplicationStatus)),
            ("loan-app-denial-reason", typeof(LoanApplicationDenialReason)),
            ("loan-status", typeof(LoanStatus)),
            ("payment-intake-status", typeof(PaymentIntakeStatus))
        };
    }

    public static class RatingLogStatus
    {
        public const string Pass = "Pass";
        public const string Fail = "Fail";
    }
}
#pragma warning restore IDE0130 // Namespace does not match folder structure
