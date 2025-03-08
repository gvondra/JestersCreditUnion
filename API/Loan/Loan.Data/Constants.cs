using JestersCreditUnion.Loan.Data.Models;

namespace JestersCreditUnion.Loan.Data.Constants
{
    public static class TableName
    {
        public const string IdentificationCard = "IdentificationCard";
        public const string Loan = "Loan";
        public const string LoanAgreement = "LoanAgreement";
        public const string LoanAgreementHistory = "LoanAgreementHistory";
        public const string LoanApplication = "LoanApplication";
        public const string LoanApplicationComment = "LoanApplicationComment";
        public const string LoanApplicationDenial = "LoanApplicationDenial";
        public const string LoanApplicationRating = "LoanApplicationRating";
        public const string LoanHistory = "LoanHistory";
        public const string Payment = "Payment";
        public const string PaymentIntake = "PaymentIntake";
        public const string PaymentTransaction = "PaymentTransaction";
        public const string Rating = "Rating";
        public const string RatingLog = "RatingLog";
        public const string Transaction = "Transaction";
    }

    public static class Column
    {
        public static string[] IdentificationCard { get; } = DataUtil.GetColumns<IdentificationCardData>();
        public static string[] Loan { get; } = DataUtil.GetColumns<LoanData>();
        public static string[] LoanAgreement { get; } = DataUtil.GetColumns<LoanAgreementData>();
        public static string[] LoanApplication { get; } = DataUtil.GetColumns<LoanApplicationData>();
        public static string[] LoanApplicationComment { get; } = DataUtil.GetColumns<LoanApplicationCommentData>();
        public static string[] LoanApplicationDenial { get; } = DataUtil.GetColumns<LoanApplicationDenialData>();
        public static string[] Payment { get; } = DataUtil.GetColumns<PaymentData>();
        public static string[] PaymentIntake { get; } = DataUtil.GetColumns<PaymentIntakeData>();
        public static string[] PaymentTransaction { get; } = DataUtil.GetColumns<PaymentTransactionData>();
        public static string[] Rating { get; } = DataUtil.GetColumns<RatingData>();
        public static string[] RatingLog { get; } = DataUtil.GetColumns<RatingLogData>();
        public static string[] Transaction { get; } = DataUtil.GetColumns<TransactionData>();
    }
}
