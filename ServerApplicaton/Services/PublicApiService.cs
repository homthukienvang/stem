using Repositories.Implementations;
using Repositories.Interfaces;

namespace Services
{
    public interface IPublicApiService
    {

    }

    public class PublicApiService : IPublicApiService
    {
        
        private readonly ICommonRepository _respository;

        public PublicApiService(ICommonRepository respository)
        {
            _respository = respository;
        }

        public PublicApiService()
            : this(new CommonRepository(new Database()))
        {
        }

    }
}