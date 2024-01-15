using JestersCreditUnion.Loan.Core;
using JestersCreditUnion.Loan.Data.Models;
using System.Globalization;

namespace Loan.Core.Test
{
    [TestClass]
    public class LoanApplicationTest
    {
        [TestMethod]
        [DataRow("1975-01-01", 49)]
        [DataRow("1974-03-01", 49)]
        [DataRow("1974-02-03", 49)]
        [DataRow("1974-02-02", 50)]
        [DataRow("1974-02-01", 50)]
        [DataRow("1974-01-31", 50)]
        [DataRow("1973-12-31", 50)]
        public void GetBorrowerAgeTest(string birthDate, int expectedAge)
        {
            LoanApplicationData data = new LoanApplicationData
            {
                ApplicationDate = new DateTime(2024, 2, 2)
            };
            LoanApplication loanApplication = new LoanApplication(
                data,
                null,
                null,
                null,
                null,
                null,
                null,
                null)
            {
                BorrowerBirthDate = DateTime.Parse(birthDate, CultureInfo.CurrentCulture)
            };
            int age = loanApplication.GetBorrowerAge();
            Assert.AreEqual(expectedAge, age);
        }

        [TestMethod]
        [DataRow("", null)]
        [DataRow("1975-01-01", 49)]
        [DataRow("1974-03-01", 49)]
        [DataRow("1974-02-03", 49)]
        [DataRow("1974-02-02", 50)]
        [DataRow("1974-02-01", 50)]
        [DataRow("1974-01-31", 50)]
        [DataRow("1973-12-31", 50)]
        public void GetCoBorrowerAgeTest(string birthDate, int? expectedAge)
        {
            LoanApplicationData data = new LoanApplicationData
            {
                ApplicationDate = new DateTime(2024, 2, 2)
            };
            LoanApplication loanApplication = new LoanApplication(
                data,
                null,
                null,
                null,
                null,
                null,
                null,
                null)
            {
                CoBorrowerBirthDate = !string.IsNullOrEmpty(birthDate) ? DateTime.Parse(birthDate, CultureInfo.CurrentCulture) : null
            };
            int? age = loanApplication.GetCoBorrowerAge();
            Assert.AreEqual(expectedAge.HasValue, age.HasValue);
            if (expectedAge.HasValue && age.HasValue)
                Assert.AreEqual(expectedAge.Value, age.Value);
        }
    }
}
