using AutoMapper;
using JestersCreditUnion.Interface.Loan.Models;
using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using Reporting = JestersCreditUnion.Loan.Framework.Reporting;

namespace LoanAPI
{
    public static class MapperConfiguration
    {
        private static readonly AutoMapper.MapperConfiguration _mapperConfiguration = new AutoMapper.MapperConfiguration(Initialize);

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
            exp.CreateMap<Reporting.LoanApplicationSummaryItem, LoanApplicationSummaryItem>();
            exp.CreateMap<LoanPaymentAmountRequest, LoanPaymentAmountResponse>();
            exp.CreateMap<Reporting.LoanPastDue, LoanPastDue>();
            exp.CreateMap<Reporting.ILoanSummary, OpenLoanSummary>();
            exp.CreateMap<Reporting.IOpenLoanSummary, OpenLoanSummaryItem>();
            exp.CreateMap<ILookup, Lookup>();
            exp.CreateMap<LoanPayment, IPayment>();
            exp.CreateMap<IPayment, LoanPayment>()
                .ForMember(p => p.Message, config => config.Ignore());
            exp.CreateMap<IPaymentIntake, PaymentIntake>();
            exp.CreateMap<PaymentIntake, IPaymentIntake>()
                .ForMember(pi => pi.Status, config => config.MapFrom((PaymentIntake pi) => (PaymentIntakeStatus)(pi.Status ?? 0)));
            exp.CreateMap<IRatingLog, RatingLog>();
            exp.CreateMap<IRating, Rating>();
            exp.CreateMap<ITransaction, Transaction>();
            exp.CreateMap<Reporting.WorkTaskCycleSummaryItem, WorkTaskCycleSummaryItem>();
        }

        public static AutoMapper.MapperConfiguration Get() => _mapperConfiguration;

        public static IMapper CreateMapper() => new Mapper(Get());
    }
}
