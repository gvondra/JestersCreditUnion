using JestersCreditUnion.CommonAPI;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace API
{
    public interface ISettingsFactory
    {
        ConfigurationSettings CreateConfiguration(Settings settings);
        LogSettings CreateLog(Settings settings);
        AuthorizationSettings CreateAuthorization(Settings settings);
        AuthorizationSettings CreateAuthorization(Settings settings, string token);
        WorkTaskSettings CreateWorkTask(Settings settings);
    }
}
#pragma warning restore IDE0130 // Namespace does not match folder structure
