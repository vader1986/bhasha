using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces;

public interface IProfileRepository
{
    IAsyncEnumerable<Profile> GetProfiles(string userId);
    Task<Profile> Add(Profile profile);
    Task Update(Profile profile);
}

