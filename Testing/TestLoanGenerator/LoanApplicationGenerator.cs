using JestersCreditUnion.Interface.Loan.Models;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class LoanApplicationGenerator : ILoanApplicationGenerator
    {
        private static readonly Random _random = new Random();
        private readonly int _count;
        private readonly INameGenerator _nameGenerator;
        private readonly IDateGenerator _dateGenerator;

        public LoanApplicationGenerator(
            INameGenerator nameGenerator,
            IDateGenerator dateGenerator,
            int count)
        {
            _nameGenerator = nameGenerator;
            _dateGenerator = dateGenerator;
            _count = count;
        }

        public LoanApplication GenerateLoanApplication()
        {
            decimal? mortage = null;
            decimal? rent = null;
            if (_random.NextDouble() < 0.5)
                mortage = (_random.Next(10000000) + 1) / 100.00M;
            else
                rent = (_random.Next(10000000) + 1) / 100.00M;
            LoanApplication loanApplication = new LoanApplication
            {
                BorrowerName = _nameGenerator.GenerateName(),
                BorrowerBirthDate = _dateGenerator.GenerateDate(dayRange: 54787),                
                BorrowerPhone = string.Format("608-555-{0:0000}", _random.Next(10000)),
                BorrowerEmployerName = _nameGenerator.GenerateName(),
                BorrowerEmploymentHireDate = _dateGenerator.GenerateDate(dayRange: 27394),
                BorrowerIncome = (_random.Next(20000000) + 1) / 100.00M,
                Amount = (_random.Next(15000000) + 1) / 100.00M,
                Purpose = "Automated test",
                MortgagePayment = mortage,
                RentPayment = rent
            };
            loanApplication.BorrowerEmailAddress = string.Format("{0}@test.org", Regex.Replace(loanApplication.BorrowerName, @"\s+", string.Empty, RegexOptions.IgnoreCase));
            return loanApplication;
        }

        public IEnumerator<LoanApplication> GetEnumerator()
        {
            return new Enumerator(this, _count);
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        internal class Enumerator : IEnumerator<LoanApplication>
        {
            private readonly ILoanApplicationGenerator _generator;
            private readonly int _count;
            private int _generated;

            public Enumerator(ILoanApplicationGenerator generator, int count)
            {
                _generator = generator;
                _count = count;
            }

            public LoanApplication Current { get; private set; }

            object IEnumerator.Current => this.Current;

            public void Dispose()
            { }

            public bool MoveNext()
            {
                bool result = false;
                if (_generated < _count)
                {
                    Current = _generator.GenerateLoanApplication();
                    _generated += 1;
                    result = true;
                }
                return result;
            }

            public void Reset()
            {
                _generated = 0;
                Current = null;
            }
        }
    }
}
