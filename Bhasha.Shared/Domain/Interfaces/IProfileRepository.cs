namespace Bhasha.Shared.Domain.Interfaces;

public interface IProfileRepository
{
    Task<Profile> Add(Profile profile, CancellationToken token = default);
    Task Update(Profile profile, CancellationToken token = default);
    IAsyncEnumerable<Profile> FindByUser(string userId, CancellationToken token = default);
}