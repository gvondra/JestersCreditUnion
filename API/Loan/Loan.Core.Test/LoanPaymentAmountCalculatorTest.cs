using JestersCreditUnion.Loan.Core;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Globalization;

namespace Loan.Core.Test
{
    [TestClass]
    public class LoanPaymentAmountCalculatorTest
    {
        private static readonly IFormatProvider _numberFormatProvider = CultureInfo.InvariantCulture.NumberFormat;

        [TestMethod]
        [DataRow("1.0", "0.01", 1, "1.00083")]
        [DataRow("100.0", "0.18", 36, "3.61524")]
        [DataRow("100000000.0", "0.09", 36, "3179973.26599")]
        public void CalculateMonthlyTest(string loanPrincipal, string annualInterestRate, int terms, string expected)
        {
            LoanPaymentAmountCalculator calculator = new LoanPaymentAmountCalculator();
            decimal amount = calculator.Calculate(decimal.Parse(loanPrincipal, _numberFormatProvider), decimal.Parse(annualInterestRate, _numberFormatProvider), (short)terms, LoanPaymentFrequency.Monthly);
            Assert.AreEqual(decimal.Parse(expected, _numberFormatProvider), amount);
        }

        [TestMethod]
        [DataRow("100.0", "0.18", 72, "1.80255")]
        public void CalculateSemimonthlyTest(string loanPrincipal, string annualInterestRate, int terms, string expected)
        {
            LoanPaymentAmountCalculator calculator = new LoanPaymentAmountCalculator();
            decimal amount = calculator.Calculate(decimal.Parse(loanPrincipal, _numberFormatProvider), decimal.Parse(annualInterestRate, _numberFormatProvider), (short)terms, LoanPaymentFrequency.Semimonthly);
            Assert.AreEqual(decimal.Parse(expected, _numberFormatProvider), amount);
        }

        [TestMethod]
        [DataRow("100.0", "0.18", 6, "22.29198")]
        public void CalculateSemiannualTest(string loanPrincipal, string annualInterestRate, int terms, string expected)
        {
            LoanPaymentAmountCalculator calculator = new LoanPaymentAmountCalculator();
            decimal amount = calculator.Calculate(decimal.Parse(loanPrincipal, _numberFormatProvider), decimal.Parse(annualInterestRate, _numberFormatProvider), (short)terms, LoanPaymentFrequency.Semiannual);
            Assert.AreEqual(decimal.Parse(expected, _numberFormatProvider), amount);
        }
    }
}
