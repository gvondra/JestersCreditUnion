﻿using AutoMapper;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Interface.Models;
using WorkTaskAPI = BrassLoon.Interface.WorkTask.Models;

namespace API
{
    public sealed class MapperConfiguration
    {
        private readonly static AutoMapper.MapperConfiguration _mapperConfiguration;

        static MapperConfiguration()
        {
            _mapperConfiguration = new AutoMapper.MapperConfiguration(Initialize);
        }

        private static void Initialize(IMapperConfigurationExpression exp)
        {
            exp.CreateMap<ILoanApplication, LoanApplication>();
            exp.CreateMap<LoanApplication, ILoanApplication>();
            exp.CreateMap<IAddress, Address>();

            exp.CreateMap<WorkTaskType, WorkTaskAPI.WorkTaskType>();
            exp.CreateMap<WorkTaskAPI.WorkTaskType, WorkTaskType>();
            exp.CreateMap<WorkTaskStatus, WorkTaskAPI.WorkTaskStatus>();
            exp.CreateMap<WorkTaskAPI.WorkTaskStatus, WorkTaskStatus>();
        }

        public static AutoMapper.MapperConfiguration Get() => _mapperConfiguration;

        public static IMapper CreateMapper() => new Mapper(Get());
    }
}
