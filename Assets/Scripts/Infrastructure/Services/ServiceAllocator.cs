namespace Infrastructure.Services
{
    internal class ServiceAllocator
    {
        private static ServiceAllocator _instance;

        public static ServiceAllocator Container => _instance ?? (_instance = new ServiceAllocator());
    
        public void RegisterSingle<TService>(TService implementation) where TService : IService
        {
            Implementation<TService>.ServiceInstance = implementation;
        }

        public TService Single<TService>() where TService : IService
        {
            return Implementation<TService>.ServiceInstance;
        }

        private static class Implementation<TService>
        {
            public static TService ServiceInstance;
        }
    }
}