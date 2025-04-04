﻿using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class UserService : IUserService
    {
        private static readonly Policy _userNameCache = Policy.Cache(new MemoryCacheProvider(new MemoryCache(new MemoryCacheOptions())), TimeSpan.FromMinutes(6));

        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public UserService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public async Task<User> Get(ISettings settings)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("User")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            List<User> users = await _restUtil.Send<List<User>>(_service, request);
            return users != null ? users[0] : null;
        }

        public Task<User> Get(ISettings settings, Guid userId)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("User/{id}")
                .AddPathParameter("id", userId.ToString("D"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<User>(_service, request);
        }

        public Task<string> GetName(ISettings settings, Guid userId)
        {
            return _userNameCache.Execute(
                context =>
                {
                    IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                    .AddPath("User/{id}/Name")
                    .AddPathParameter("id", userId.ToString("D"))
                    .AddJwtAuthorizationToken(settings.GetToken)
                    ;
                    return _restUtil.Send<string>(_service, request);
                },
                new Context(userId.ToString("N")));
        }

        public Task<List<User>> Search(ISettings settings, string emailAddress)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("User")
                .AddQueryParameter("emailAddress", emailAddress ?? string.Empty)
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<User>>(_service, request);
        }

        public Task<User> Update(ISettings settings, Guid userId, User user)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put, user)
                .AddPath("User/{id}")
                .AddPathParameter("id", userId.ToString("D"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<User>(_service, request);
        }

        public Task<User> Update(ISettings settings, User user)
        {
            if (!user.UserId.HasValue || user.UserId.Value.Equals(Guid.Empty))
                throw new ArgumentException($"{nameof(user.UserId)} is null");
            return Update(settings, user.UserId.Value, user);
        }
    }
}
