using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bhasha.Infrastructure.EntityFramework;

public class EntityFrameworkProfileRepository(AppDbContext context) : IProfileRepository
{
    public async Task<Profile> Add(Profile profile, CancellationToken token = default)
    {
        var result = await context.Profiles
            .AddAsync(Converter.Convert(profile), token);

        await context
            .SaveChangesAsync(token);

        return Converter
            .Convert(result.Entity);
    }

    public async Task Update(Profile profile, CancellationToken token = default)
    {
        var result = await context.Profiles
            .FirstAsync(x => x.Id == profile.Id, token);

        context
            .Remove(result);

        await context
            .AddAsync(Converter.Convert(profile), token);

        await context
            .SaveChangesAsync(token);
    }

    public IAsyncEnumerable<Profile> FindByUser(string userId, CancellationToken token = default)
    {
        return context.Profiles
            .Where(x => x.UserId == userId)
            .Select(x => Converter.Convert(x))
            .ToAsyncEnumerable();
    }
}