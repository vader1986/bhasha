using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Bhasha.Identity.Mongo
{
    public class CustomResourceStore : IResourceStore
    {
        protected IRepository _repository;

        public CustomResourceStore(IRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var resources = _repository.Where<ApiResource>(x => apiResourceNames.Contains(x.Name));
            return Task.FromResult(resources.AsEnumerable());
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = _repository.Where<ApiResource>(x => x.Scopes.Any(scope => scopeNames.Contains(scope)));
            return Task.FromResult(resources.AsEnumerable());
        }

        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var scopes = _repository.Where<ApiScope>(x => scopeNames.Contains(x.Name));
            return Task.FromResult(scopes.AsEnumerable());
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = _repository.Where<IdentityResource>(x => scopeNames.Contains(x.Name));
            return Task.FromResult(resources.AsEnumerable());
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var resources = new Resources(
                _repository.All<IdentityResource>(),
                _repository.All<ApiResource>(),
                _repository.All<ApiScope>());
            return Task.FromResult(resources);
        }
    }
}
