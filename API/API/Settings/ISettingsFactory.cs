using JestersCreditUnion.CommonAPI;

namespace API
{
    public interface ISettingsFactory
    {
        ConfigurationSettings CreateConfiguration(Settings settings);
        CoreSettings CreateCore(Settings settings);
        LogSettings CreateLog(Settings settings);
        AuthorizationSettings CreateAuthorization(Settings settings);
        AuthorizationSettings CreateAuthorization(Settings settings, string token);
        WorkTaskSettings CreateWorkTask(Settings settings);
    }
}
