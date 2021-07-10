using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Bhasha.Identity.Mongo
{
    public class CustomClientStore : IClientStore
    {
        protected IRepository _repository;

        public CustomClientStore(IRepository repository)
        {
            _repository = repository;
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _repository.Single<Client>(c => c.ClientId == clientId);

            return Task.FromResult(client);
        }
    }
}
