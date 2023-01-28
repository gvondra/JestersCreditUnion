using BrassLoon.DataClient;

namespace JestersCreditUnion.Data.Internal
{
    public abstract class DataFactoryBase<T> 
    {
        private readonly ISqlDbProviderFactory _providerFactory;
        private readonly IGenericDataFactory<T> _genericDataFactory;

        protected DataFactoryBase(ISqlDbProviderFactory providerFactory)
        {
            _providerFactory = providerFactory;
            _genericDataFactory = new GenericDataFactory<T>();
        }

        protected abstract T Create();

        protected ISqlDbProviderFactory ProviderFactory => _providerFactory;
            
        protected IGenericDataFactory<T> GenericDataFactory => _genericDataFactory;
    }
}
