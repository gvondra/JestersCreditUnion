namespace JestersCreditUnion.Loan.Data.Internal
{
    public abstract class DataFactoryBase<T>
        where T : new()
    {
        private readonly IDbProviderFactory _providerFactory;
        private readonly IGenericDataFactory<T> _dataFactory;

        protected DataFactoryBase(IDbProviderFactory providerFactory, IGenericDataFactory<T> dataFactory)
        {
            _providerFactory = providerFactory;
            _dataFactory = dataFactory;
        }

        protected IDbProviderFactory ProviderFactory => _providerFactory;
        protected IGenericDataFactory<T> DataFactory => _dataFactory;

        protected static T Create() => new T();
    }
}
