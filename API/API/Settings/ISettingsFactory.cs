using JestersCreditUnion.CommonAPI;

namespace API
{
    public interface ISettingsFactory
    {
        CoreSettings CreateCore(Settings settings);
        LogSettings CreateLog(Settings settings);
        AuthorizationSettings CreateAuthorization(Settings settings);
        AuthorizationSettings CreateAuthorization(Settings settings, string token);
    }
}
