using JestersCreditUnion.Core;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Test
{
    [TestClass]
    public class LoanPaymentProcessorTest
    {
        [TestMethod]
        public async Task ProcessFirstPaymentIsLateTest()
        {
            Mock<ISettings> settings = new Mock<ISettings>();
            Mock<IPaymentSaver> paymentSaver = new Mock<IPaymentSaver>();

            List<IPaymentTransaction> paymentTransactions = new List<IPaymentTransaction>();

            Mock<IPayment> payment = new Mock<IPayment>();
            payment.SetupAllProperties();
            payment.Object.Status = PaymentStatus.Unprocessed;
            payment.Object.Amount = 10.0M;
            payment.SetupGet(p => p.Transactions).Returns(paymentTransactions);
            payment.SetupGet(p => p.Date).Returns(new DateTime(2022, 08, 05));
            payment.SetupGet(p => p.TransactionNumber).Returns("xn");
            payment.Setup(p => p.CreatePaymentTransaction(It.IsNotNull<ILoan>(), It.IsAny<DateTime>(), It.IsAny<TransactionType>(), It.IsAny<decimal>()))
                .Returns((ILoan l, DateTime d, TransactionType t, decimal amt) =>
                {
                    Mock<IPaymentTransaction> transaction = new Mock<IPaymentTransaction>();
                    transaction.SetupAllProperties();
                    transaction.SetupGet(t => t.Amount).Returns(amt);
                    transaction.SetupGet(t => t.IsNew).Returns(true);
                    transaction.SetupGet(t => t.Date).Returns(d);
                    transaction.SetupGet(t => t.Type).Returns(t);
                    return transaction.Object;
                });

            Mock<ILoanAgreement> agreement = new Mock<ILoanAgreement>();
            agreement.SetupAllProperties();
            agreement.Object.PaymentFrequency = LoanPaymentFrequency.Monthly;
            agreement.Object.OriginalAmount = 1024.00M;
            agreement.Object.InterestRate = 0.05M;

            Mock<ILoan> loan = new Mock<ILoan>();
            loan.SetupAllProperties();
            loan.Object.InitialDisbursementDate = new DateTime(2022, 06, 20);
            loan.Object.FirstPaymentDue = PaymentDateCalculator.FirstPaymentDate(loan.Object.InitialDisbursementDate.Value, agreement.Object.PaymentFrequency);
            loan.Object.NextPaymentDue = loan.Object.FirstPaymentDue;
            loan.SetupGet(l => l.Agreement).Returns(agreement.Object);
            loan.Setup(l => l.GetPayments(settings.Object)).Returns(Task.FromResult<IEnumerable<IPayment>>(new List<IPayment> { payment.Object }));

            LoanPaymentProcessor loanPaymentProcessor = new LoanPaymentProcessor(paymentSaver.Object);
            await loanPaymentProcessor.Process(settings.Object, loan.Object);
            Assert.AreEqual(2, paymentTransactions.Where(t => t.Type == TransactionType.InterestPayment).Count());
            Assert.AreEqual(1, paymentTransactions.Where(t => t.Type == TransactionType.PrincipalPayment).Count());
            Assert.AreEqual(1, paymentTransactions.Where(t => t.TermNumber == 1).Count());
            Assert.AreEqual(2, paymentTransactions.Where(t => t.TermNumber == 2).Count());
            Assert.AreEqual(new DateTime(2022, 10, 1), loan.Object.NextPaymentDue.Value);
            paymentSaver.Verify(s => s.Update(settings.Object, It.IsNotNull<IEnumerable<IPayment>>()), Times.Once);
        }
    }
}
