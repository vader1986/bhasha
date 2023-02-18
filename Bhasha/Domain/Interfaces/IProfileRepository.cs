namespace Bhasha.Domain.Interfaces;

public interface IProfileRepository
{
    Task<Profile> Add(Profile profile);
    Task Update(Profile profile);
    IAsyncEnumerable<Profile> FindByUser(string userId);
}