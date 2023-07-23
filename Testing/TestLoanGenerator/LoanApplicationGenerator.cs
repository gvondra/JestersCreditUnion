using JestersCreditUnion.Interface.Models;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanApplicationGenerator : ILoanApplicationGenerator
    {
        private readonly INameGenerator _nameGenerator;
        private readonly IDateGenerator _dateGenerator;

        public LoanApplicationGenerator(INameGenerator nameGenerator, IDateGenerator dateGenerator)
        {
            _nameGenerator = nameGenerator;
            _dateGenerator = dateGenerator;
        }

        public LoanApplication GenerateLoanApplication()
        {
            LoanApplication loanApplication = new LoanApplication
            {
                BorrowerName = _nameGenerator.GenerateName(),
                BorrowerBirthDate = _dateGenerator.GenerateDate(yearRange: 250)
            };
            return loanApplication;
        }
    }
}
