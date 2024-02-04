using JestersCreditUnion.Loan.Framework.Reporting;
using JestersCreditUnion.Loan.Reporting.Core;
using Moq;
using System.Collections.Generic;

namespace Loan.Reporting.Core.Test
{
    [TestClass]
    public class LoanSummaryBuilderTest
    {
        [TestMethod]
        public void BuildOpenLoanSummaryEmptyListTest()
        {
            List<IOpenLoanSummary> openLoanSummaries = new List<IOpenLoanSummary>();
            LoanSummaryBuilder builder = new LoanSummaryBuilder();
            ILoanSummary loanSummary = builder.BuildOpenLoanSummary(openLoanSummaries);
            Assert.IsNotNull(loanSummary);
            Assert.AreEqual(0, loanSummary.LoanCount);
            Assert.AreEqual(0, loanSummary.Count60DaysOverdue);
            Assert.AreEqual(0.0, loanSummary.MedianBalance);
            Assert.AreEqual(openLoanSummaries, loanSummary.Items);
        }

        [TestMethod]
        public void BuildOpenLoanSummarySingleItemTest()
        {
            List<IOpenLoanSummary> openLoanSummaries = [
                CreateOpenLoanSummary("1", 2.3M, new DateTime(2023, 2, 2)).Object
            ];
            LoanSummaryBuilder builder = new LoanSummaryBuilder();
            ILoanSummary loanSummary = builder.BuildOpenLoanSummary(openLoanSummaries);
            Assert.IsNotNull(loanSummary);
            Assert.AreEqual(1, loanSummary.LoanCount);
            Assert.AreEqual(1, loanSummary.Count60DaysOverdue);
            Assert.AreEqual(2.3, loanSummary.MedianBalance);
            Assert.AreEqual(openLoanSummaries, loanSummary.Items);
        }

        [TestMethod]
        public void BuildOpenLoanSummaryEvenCountTest()
        {
            List<IOpenLoanSummary> openLoanSummaries = [
                CreateOpenLoanSummary("1", 2.3M, new DateTime(2024, 2, 2)).Object,
                CreateOpenLoanSummary("2", 4.0M, new DateTime(2024, 2, 2)).Object,
                CreateOpenLoanSummary("3", 6.0M, new DateTime(2024, 2, 2)).Object,
                CreateOpenLoanSummary("4", 7.5M, new DateTime(2024, 2, 2)).Object
            ];
            LoanSummaryBuilder builder = new LoanSummaryBuilder();
            ILoanSummary loanSummary = builder.BuildOpenLoanSummary(openLoanSummaries);
            Assert.IsNotNull(loanSummary);
            Assert.AreEqual(4, loanSummary.LoanCount);
            Assert.AreEqual(5.0, loanSummary.MedianBalance);
            Assert.AreEqual(openLoanSummaries, loanSummary.Items);
        }

        [TestMethod]
        public void BuildOpenLoanSummaryOddCountTest()
        {
            List<IOpenLoanSummary> openLoanSummaries = [
                CreateOpenLoanSummary("1", 2.3M, new DateTime(2024, 2, 2)).Object,
                CreateOpenLoanSummary("2", 4.0M, new DateTime(2024, 2, 2)).Object,
                CreateOpenLoanSummary("3", 6.0M, new DateTime(2024, 2, 2)).Object,
                CreateOpenLoanSummary("4", 7.5M, new DateTime(2024, 2, 2)).Object,
                CreateOpenLoanSummary("5", 17.0M, new DateTime(2024, 2, 2)).Object
            ];
            LoanSummaryBuilder builder = new LoanSummaryBuilder();
            ILoanSummary loanSummary = builder.BuildOpenLoanSummary(openLoanSummaries);
            Assert.IsNotNull(loanSummary);
            Assert.AreEqual(5, loanSummary.LoanCount);
            Assert.AreEqual(6.0, loanSummary.MedianBalance);
            Assert.AreEqual(openLoanSummaries, loanSummary.Items);
        }

        private static Mock<IOpenLoanSummary> CreateOpenLoanSummary(string number, decimal balance, DateTime nextPaymentDue)
        {
            Mock<IOpenLoanSummary> openLoanSummary = new Mock<IOpenLoanSummary>();
            openLoanSummary.SetupGet(s => s.Number).Returns(number);
            openLoanSummary.SetupGet(s => s.Balance).Returns(balance);
            openLoanSummary.SetupGet(s => s.NextPaymentDue).Returns(nextPaymentDue);
            return openLoanSummary;
        }
    }
}
