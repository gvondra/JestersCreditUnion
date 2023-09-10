using JestersCreditUnion.Loan.Core;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Linq;

namespace Loan.Core.Test
{
    [TestClass]
    public class PaymentDateCalculatorTest
    {
        [TestMethod]
        public void AnnualPaymentTest()
        {
            LoanPaymentFrequency frequency = LoanPaymentFrequency.Annual;
            DateTime[] paymentDates = new DateTime[12];
            DateTime dueAfter = new DateTime(2020, 02, 11);
            paymentDates[0] = PaymentDateCalculator.FirstPaymentDate(dueAfter, frequency);
            for (int i = 1; i < paymentDates.Length; i += 1)
            {
                paymentDates[i] = PaymentDateCalculator.NextPaymentDate(paymentDates[i - 1], frequency);
            }
            for (int i = 0; i < paymentDates.Length; i += 1)
            {
                Assert.AreEqual(3, paymentDates[i].Month);
                Assert.AreEqual(1, paymentDates[i].Day);
            }
            Assert.AreEqual(2032, paymentDates.Max(d => d.Year));
        }

        [TestMethod]
        public void SemiAnnualPaymentTest()
        {
            LoanPaymentFrequency frequency = LoanPaymentFrequency.Semiannual;
            DateTime[] paymentDates = new DateTime[12];
            DateTime dueAfter = new DateTime(2020, 02, 11);
            paymentDates[0] = PaymentDateCalculator.FirstPaymentDate(dueAfter, frequency);
            for (int i = 1; i < paymentDates.Length; i += 1)
            {
                paymentDates[i] = PaymentDateCalculator.NextPaymentDate(paymentDates[i - 1], frequency);
            }
            for (int i = 0; i < paymentDates.Length; i += 2)
            {
                Assert.AreEqual(9, paymentDates[i].Month);
                Assert.AreEqual(1, paymentDates[i].Day);
            }
            for (int i = 1; i < paymentDates.Length; i += 2)
            {
                Assert.AreEqual(3, paymentDates[i].Month);
                Assert.AreEqual(1, paymentDates[i].Day);
            }
            Assert.AreEqual(2026, paymentDates.Max(d => d.Year));
        }

        [TestMethod]
        public void MonthlyPaymentTest()
        {
            LoanPaymentFrequency frequency = LoanPaymentFrequency.Monthly;
            DateTime[] paymentDates = new DateTime[12];
            DateTime dueAfter = new DateTime(2020, 02, 11);
            paymentDates[0] = PaymentDateCalculator.FirstPaymentDate(dueAfter, frequency);
            for (int i = 1; i < paymentDates.Length; i += 1)
            {
                paymentDates[i] = PaymentDateCalculator.NextPaymentDate(paymentDates[i - 1], frequency);
            }
            for (int i = 0; i < paymentDates.Length; i += 1)
            {
                Assert.AreEqual(1, paymentDates[i].Day);
            }
            Assert.AreEqual(new DateTime(2021, 3, 1), paymentDates.Max());
        }

        [TestMethod]
        public void SemiMonthlyPaymentTest()
        {
            LoanPaymentFrequency frequency = LoanPaymentFrequency.Semimonthly;
            DateTime[] paymentDates = new DateTime[12];
            DateTime dueAfter = new DateTime(2020, 02, 11);
            paymentDates[0] = PaymentDateCalculator.FirstPaymentDate(dueAfter, frequency);
            for (int i = 1; i < paymentDates.Length; i += 1)
            {
                paymentDates[i] = PaymentDateCalculator.NextPaymentDate(paymentDates[i - 1], frequency);
            }
            for (int i = 0; i < paymentDates.Length; i += 2)
            {
                Assert.AreEqual(15, paymentDates[i].Day);
            }
            for (int i = 1; i < paymentDates.Length; i += 2)
            {
                Assert.AreEqual(1, paymentDates[i].Day);
            }
            Assert.AreEqual(new DateTime(2020, 9, 1), paymentDates.Max());
        }

        [TestMethod]
        public void FortnightlyPaymentTest()
        {
            LoanPaymentFrequency frequency = LoanPaymentFrequency.Fortnightly;
            DateTime[] paymentDates = new DateTime[12];
            DateTime dueAfter = new DateTime(2020, 02, 11);
            paymentDates[0] = PaymentDateCalculator.FirstPaymentDate(dueAfter, frequency);
            for (int i = 1; i < paymentDates.Length; i += 1)
            {
                paymentDates[i] = PaymentDateCalculator.NextPaymentDate(paymentDates[i - 1], frequency);
            }
            Assert.AreEqual(new DateTime(2020, 3, 15), paymentDates[0]);
            for (int i = 1; i < paymentDates.Length; i += 2)
            {
                Assert.AreEqual(paymentDates[i - 1].AddDays(14), paymentDates[i]);
                Assert.AreEqual(DayOfWeek.Sunday, paymentDates[i].DayOfWeek);
            }
            Assert.AreEqual(new DateTime(2020, 8, 16), paymentDates.Max());
        }
    }
}
