using System;
using System.Security.Cryptography;
using System.Text;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public static class Hash
    {
        public static byte[] HashLoanAgreement(
            DateTime createDate,
            DateTime? agreementDate,
            decimal interestRate,
            decimal paymentAmount)
        {
            StringBuilder text = new StringBuilder();
            text.Append(createDate.ToString("O"));
            text.Append(",");
            text.Append(agreementDate.HasValue ? agreementDate.Value.ToString("O") : string.Empty);
            text.Append(",");
            text.Append(interestRate);
            text.Append(",");
            text.Append(paymentAmount);
            return HashText(text.ToString());
        }

        private static byte[] HashText(string text)
        {
            using SHA512 sha = SHA512.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(text));
        }
    }
}
