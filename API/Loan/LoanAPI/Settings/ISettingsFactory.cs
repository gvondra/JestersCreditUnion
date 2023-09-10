using JestersCreditUnion.CommonAPI;
#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace LoanAPI
{
    public interface ISettingsFactory
    {
        CoreSettings CreateCore();
        AuthorizationSettings CreateAuthorization();
    }
}
#pragma warning restore IDE0130 // Namespace does not match folder structure