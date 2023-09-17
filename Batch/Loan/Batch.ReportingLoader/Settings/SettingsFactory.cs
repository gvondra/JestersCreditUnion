namespace JestersCreditUnion.Batch.ReportingLoader
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly Settings _settings;

        public SettingsFactory(Settings settings)
        {
            _settings = settings;
        }

        public DataSettings CreateData() => new DataSettings(_settings);
    }
}
