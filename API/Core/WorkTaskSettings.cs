﻿using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;
using AccountAPI = BrassLoon.Interface.Account;
using WorkTaskAPI = BrassLoon.Interface.WorkTask;

namespace JestersCreditUnion.Core
{
    public class WorkTaskSettings : WorkTaskAPI.ISettings
    {
        private readonly ISettings _settings;
        private readonly AccountAPI.ITokenService _tokenService;

        internal WorkTaskSettings(ISettings settings, AccountAPI.ITokenService tokenService)
        {
            _settings = settings;
            _tokenService = tokenService;
        }

        public string BaseAddress => _settings.BrassLoonWorkTaskApiBaseAddress;

        public async Task<string> GetToken()
        {
            if (string.IsNullOrEmpty(_settings.BrassLoonAccountApiBaseAddress))
                throw new ArgumentException("Brass Loon Account base address property value not set");
            if (!_settings.BrassLoonClientId.HasValue || _settings.BrassLoonClientId.Value.Equals(Guid.Empty))
                throw new ArgumentException("Brass Loon Account client id property value not set");
            if (string.IsNullOrEmpty(_settings.BrassLoonClientSecret))
                throw new ArgumentException("Brass Loon Account client secret property value not set");
            AccountSettings settings = new AccountSettings(_settings);
            return await _tokenService.CreateClientCredentialToken(settings, _settings.BrassLoonClientId.Value, _settings.BrassLoonClientSecret);
        }
    }
}
