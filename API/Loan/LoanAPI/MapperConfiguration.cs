using AutoMapper;
using JestersCreditUnion.Interface.Loan.Models;
using JestersCreditUnion.Loan.Framework;

namespace LoanAPI
{
    public static class MapperConfiguration
    {
        private static readonly AutoMapper.MapperConfiguration _mapperConfiguration;

        static MapperConfiguration()
        {
            _mapperConfiguration = new AutoMapper.MapperConfiguration(Initialize);
        }

        private static void Initialize(IMapperConfigurationExpression exp)
        {
            exp.CreateMap<IAddress, Address>();
            exp.CreateMap<IAmortizationItem, AmortizationItem>();
            exp.CreateMap<ILoan, Loan>();
            exp.CreateMap<Loan, ILoan>()
                .ForMember(l => l.Agreement, opt => opt.Ignore());
            exp.CreateMap<ILoanAgreement, LoanAgreement>();
            exp.CreateMap<LoanAgreement, ILoanAgreement>();
            exp.CreateMap<ILoanApplication, LoanApplication>()
                .ForMember(la => la.Comments, opt => opt.Ignore()); // comments require authorization
            exp.CreateMap<LoanApplication, ILoanApplication>()
                .ForMember(la => la.Comments, opt => opt.Ignore()); // prevent incoming comments from overwriting saved comments
            exp.CreateMap<ILoanApplicationComment, LoanApplicationComment>();
            exp.CreateMap<LoanApplicationComment, ILoanApplicationComment>();
            exp.CreateMap<ILoanApplicationDenial, LoanApplicationDenial>();
            exp.CreateMap<LoanApplicationDenial, ILoanApplicationDenial>();
            exp.CreateMap<JestersCreditUnion.Loan.Framework.Reporting.LoanApplicationSummaryItem, LoanApplicationSummaryItem>();
            exp.CreateMap<LoanPaymentAmountRequest, LoanPaymentAmountResponse>();
            exp.CreateMap<ILookup, Lookup>();
            exp.CreateMap<LoanPayment, IPayment>();
            exp.CreateMap<IPayment, LoanPayment>()
                .ForMember(p => p.Message, config => config.Ignore());
            exp.CreateMap<ITransaction, Transaction>();
            exp.CreateMap<JestersCreditUnion.Loan.Framework.Reporting.WorkTaskCycleSummaryItem, WorkTaskCycleSummaryItem>();
        }

        public static AutoMapper.MapperConfiguration Get() => _mapperConfiguration;

        public static IMapper CreateMapper() => new Mapper(Get());
    }
}
