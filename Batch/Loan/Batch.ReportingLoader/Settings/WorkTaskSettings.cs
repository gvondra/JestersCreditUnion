﻿using BrassLoon.Interface.WorkTask;
using System;
using System.Threading.Tasks;
using Account = BrassLoon.Interface.Account;

namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class WorkTaskSettings : ISettings
    {
        private readonly Settings _settings;
        private readonly Account.ITokenService _tokenService;

        public WorkTaskSettings(Settings settings, Account.ITokenService tokenService)
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
