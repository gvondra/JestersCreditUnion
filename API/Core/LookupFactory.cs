using BrassLoon.Interface.Config;
using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Models = BrassLoon.Interface.Config.Models;

namespace JestersCreditUnion.Core
{
    public class LookupFactory : ILookupFactory
    {
        private readonly SettingsFactory _settingsFactory;
        private readonly ILookupService _lookupService;

        public LookupFactory(SettingsFactory settingsFactory, ILookupService lookupService)
        {
            _settingsFactory = settingsFactory;
            _lookupService = lookupService;
        }

        public async Task<ILookup> GetLookup(JestersCreditUnion.Framework.ISettings settings, string code)
        {
            ILookup result = null;
            Models.Lookup data = null;
            ConfigurationSettings configurationSettings = _settingsFactory.CreateConfiguration(settings);
            try
            {
                data = await _lookupService.GetByCode(configurationSettings, settings.ConfigDomainId.Value, code);
            }
            catch (BrassLoon.RestClient.Exceptions.RequestError ex)
            {
                if (ex.StatusCode != HttpStatusCode.NotFound)
                    throw;
            }
            Type type = EnumerationDesriptionLookup.Map
                .Where(m => string.Equals(m.Item1, code, StringComparison.OrdinalIgnoreCase))
                .Select(m => m.Item2)
                .FirstOrDefault();
            if (type != null)
            {
                if (data == null)
                {
                    data = new Models.Lookup
                    {
                        Code = code,
                        Data = new Dictionary<string, string>(),
                        CreateTimestamp = DateTime.UtcNow,
                        UpdateTimestamp = DateTime.UtcNow,
                        DomainId = settings.ConfigDomainId.Value
                    };
                }
                foreach (object value in Enum.GetValues(type))
                {
                    string key = (Convert.ToInt32(value)).ToString();
                    if (!data.Data.ContainsKey(key))
                    {
                        data.Data[key] = Enum.GetName(type, value);
                    }
                }
            }
            if (data != null)
                result = new Lookup(data);
            return result;
        }

        public async Task<ILookup> GetLookup(Framework.ISettings settings, Type type)
        {
            string code = EnumerationDesriptionLookup.Map
                .Where(m => type.Equals(m.Item2))
                .Select(m => m.Item1)
                .FirstOrDefault();
            ILookup result = null;
            if (!string.IsNullOrEmpty(code))
            {
                result = await GetLookup(settings, code);
            }
            return result;
        }
    }
}
