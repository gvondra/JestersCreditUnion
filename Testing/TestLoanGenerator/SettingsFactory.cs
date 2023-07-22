namespace JestersCreditUnion.Testing.LoanGenerator
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly Settings _settings;

        public SettingsFactory(Settings settings)
        {
            _settings = settings;
        }
    }
}
