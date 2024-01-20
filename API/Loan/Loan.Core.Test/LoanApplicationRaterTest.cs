using JestersCreditUnion.Loan.Core;
using JestersCreditUnion.Loan.Framework;
using Moq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Loan.Core.Test
{
    [TestClass]
    public class LoanApplicationRaterTest
    {
        [TestMethod]
        public async Task MinAgeTest()
        {
            Mock<IRatingFactory> ratingFactory = CreateRatingFactory();

            Mock<ILoanApplication> loanApplication = new Mock<ILoanApplication>();
            loanApplication.Setup(a => a.GetBorrowerAge()).Returns(17);

            LoanApplicationRaterFactory factory = new LoanApplicationRaterFactory(ratingFactory.Object);
            ILoanApplicationRater rater = await factory.Create();
            Assert.IsNotNull(rater);
            Assert.IsInstanceOfType<LoanApplicationRater>(rater);

            IRating rating = rater.Rate(loanApplication.Object);
            Assert.IsNotNull(rating);
            Assert.AreEqual(0.668, rating.Value);
            Assert.AreEqual(6, rating.RatingLogs.Count);
        }

        [TestMethod]
        public async Task MaxAgeTest()
        {
            Mock<IRatingFactory> ratingFactory = CreateRatingFactory();

            Mock<ILoanApplication> loanApplication = new Mock<ILoanApplication>();
            loanApplication.Setup(a => a.GetBorrowerAge()).Returns(100);

            LoanApplicationRaterFactory factory = new LoanApplicationRaterFactory(ratingFactory.Object);
            ILoanApplicationRater rater = await factory.Create();
            Assert.IsNotNull(rater);
            Assert.IsInstanceOfType<LoanApplicationRater>(rater);

            IRating rating = rater.Rate(loanApplication.Object);
            Assert.IsNotNull(rating);
            Assert.AreEqual(0.668, rating.Value);
            Assert.AreEqual(6, rating.RatingLogs.Count);
        }

        private static Mock<IRatingFactory> CreateRatingFactory()
        {
            Mock<IRatingFactory> ratingFactory = new Mock<IRatingFactory>();
            ratingFactory.Setup(f => f.Create(It.IsAny<double>(), It.IsAny<IEnumerable<IRatingLog>>()))
                .Returns((double v, IEnumerable<IRatingLog> logs) =>
                {
                    Mock<IRating> rating = new Mock<IRating>();
                    rating.SetupAllProperties();
                    rating.Object.Value = v;
                    rating.SetupGet(r => r.RatingLogs).Returns(ImmutableList<IRatingLog>.Empty.AddRange(logs));
                    return rating.Object;
                });
            ratingFactory.Setup(f => f.CreateLog(It.IsAny<double>(), It.IsAny<string>()))
                .Returns((double v, string desc) =>
                {
                    Mock<IRatingLog> ratingLog = new Mock<IRatingLog>();
                    ratingLog.SetupAllProperties();
                    ratingLog.Object.Value = v;
                    ratingLog.Object.Description = desc;
                    return ratingLog.Object;
                });
            return ratingFactory;
        }
    }
}
