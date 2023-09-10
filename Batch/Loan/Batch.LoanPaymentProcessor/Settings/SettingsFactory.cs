namespace JestersCreditUnion.Batch.LoanPaymentProcessor
{
    public class SettingsFactory
    {
        private readonly Settings _settings;

        public SettingsFactory(Settings settings)
        {
            _settings = settings;
        }

        public CoreSettings CreateCore() => new CoreSettings(_settings);
    }
}
