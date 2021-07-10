using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Bhasha.Identity.Mongo
{
    public class CustomPersistedGrantStore : IPersistedGrantStore
    {
        protected IRepository _repository;

        public CustomPersistedGrantStore(IRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            var grants = _repository
                .Where<PersistedGrant>(
                    x => (string.IsNullOrEmpty(filter.ClientId) || x.ClientId == filter.ClientId) &&
                         (string.IsNullOrEmpty(filter.SessionId) || x.ClientId == filter.SessionId) &&
                         (string.IsNullOrEmpty(filter.SubjectId) || x.ClientId == filter.SubjectId) &&
                         (string.IsNullOrEmpty(filter.Type) || x.ClientId == filter.Type));

            return Task.FromResult(grants.AsEnumerable());
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            return Task.FromResult(_repository.Single<PersistedGrant>(x => x.Key == key));
        }

        public Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            _repository.Delete<PersistedGrant>(
                x => (string.IsNullOrEmpty(filter.ClientId) || x.ClientId == filter.ClientId) &&
                     (string.IsNullOrEmpty(filter.SessionId) || x.ClientId == filter.SessionId) &&
                     (string.IsNullOrEmpty(filter.SubjectId) || x.ClientId == filter.SubjectId) &&
                     (string.IsNullOrEmpty(filter.Type) || x.ClientId == filter.Type));

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _repository.Delete<PersistedGrant>(x => x.Key == key);

            return Task.CompletedTask;
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            _repository.Add(grant);

            return Task.CompletedTask;
        }
    }
}
