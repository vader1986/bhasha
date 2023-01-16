namespace Bhasha.Web.Domain.Interfaces;

public interface IProfileRepository
{
    IAsyncEnumerable<Profile> GetProfiles(string userId);
    Task<Profile> Add(Profile profile);
    Task Update(Profile profile);
}

