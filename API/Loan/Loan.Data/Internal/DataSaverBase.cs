namespace JestersCreditUnion.Loan.Data.Internal
{
    public abstract class DataSaverBase
    {
#pragma warning disable SA1401 // Fields should be private
        protected readonly IDbProviderFactory _providerFactory;
#pragma warning restore SA1401 // Fields should be private

        protected DataSaverBase(IDbProviderFactory providerFactory)
        {
            _providerFactory = providerFactory;
        }
    }
}
