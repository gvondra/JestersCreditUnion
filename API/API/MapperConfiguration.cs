using AutoMapper;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Interface.Models;
using AuthorizationAPI = BrassLoon.Interface.Authorization.Models;
using ConfigAPI = BrassLoon.Interface.Config.Models;
using LogAPI = BrassLoon.Interface.Log.Models;
using WorkTaskAPI = BrassLoon.Interface.WorkTask.Models;

namespace API
{
    public sealed class MapperConfiguration
    {
        private static readonly AutoMapper.MapperConfiguration _mapperConfiguration;

        static MapperConfiguration()
        {
            _mapperConfiguration = new AutoMapper.MapperConfiguration(Initialize);
        }

        private static void Initialize(IMapperConfigurationExpression exp)
        {
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
            exp.CreateMap<LoanPaymentAmountRequest, LoanPaymentAmountResponse>();
            exp.CreateMap<IAddress, Address>();
            exp.CreateMap<ILookup, Lookup>();

            exp.CreateMap<AuthorizationAPI.AppliedRole, AppliedRole>();
            exp.CreateMap<AppliedRole, AuthorizationAPI.AppliedRole>();
            exp.CreateMap<AuthorizationAPI.Role, Role>();
            exp.CreateMap<Role, AuthorizationAPI.Role>();
            exp.CreateMap<AuthorizationAPI.User, User>();
            exp.CreateMap<User, AuthorizationAPI.User>();

            exp.CreateMap<ConfigAPI.Lookup, Lookup>();
            exp.CreateMap<Lookup, ConfigAPI.Lookup>();

            exp.CreateMap<LogAPI.Exception, Exception>();
            exp.CreateMap<LogAPI.EventId, EventId>();
            exp.CreateMap<LogAPI.Metric, Metric>();
            exp.CreateMap<LogAPI.Trace, Trace>();

            exp.CreateMap<ClaimWorkTaskResponse, WorkTaskAPI.ClaimWorkTaskResponse>();
            exp.CreateMap<WorkTaskAPI.ClaimWorkTaskResponse, ClaimWorkTaskResponse>();
            exp.CreateMap<WorkGroup, WorkTaskAPI.WorkGroup>();
            exp.CreateMap<WorkTaskAPI.WorkGroup, WorkGroup>();
            exp.CreateMap<WorkTask, WorkTaskAPI.WorkTask>();
            exp.CreateMap<WorkTaskAPI.WorkTask, WorkTask>();
            exp.CreateMap<WorkTaskContext, WorkTaskAPI.WorkTaskContext>();
            exp.CreateMap<WorkTaskAPI.WorkTaskContext, WorkTaskContext>();
            exp.CreateMap<WorkTaskType, WorkTaskAPI.WorkTaskType>();
            exp.CreateMap<WorkTaskAPI.WorkTaskType, WorkTaskType>();
            exp.CreateMap<WorkTaskStatus, WorkTaskAPI.WorkTaskStatus>();
            exp.CreateMap<WorkTaskAPI.WorkTaskStatus, WorkTaskStatus>();
        }

        public static AutoMapper.MapperConfiguration Get() => _mapperConfiguration;

        public static IMapper CreateMapper() => new Mapper(Get());
    }
}
