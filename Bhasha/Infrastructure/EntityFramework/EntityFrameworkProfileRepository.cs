using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Profile = Bhasha.Domain.Profile;

namespace Bhasha.Infrastructure.EntityFramework;

public sealed class EntityFrameworkProfileRepository(AppDbContext context) : IProfileRepository
{
    public async Task<Profile> Add(Profile profile, CancellationToken token = default)
    {
        var result = await context.Profiles
            .AddAsync(profile.ToEntityFramework(), token);

        await context.SaveChangesAsync(token);

        return result.Entity.ToDomain();
    }

    public async Task Delete(ProfileKey key, CancellationToken token = default)
    {
        var dto = await context.Profiles
            .SingleAsync(x => x.UserId == key.UserId && 
                              x.Native == key.Native &&
                              x.Target == key.Target, token);

        context.Profiles.Remove(dto);
        
        await context.SaveChangesAsync(token);
    }

    public async Task Update(Profile profile, CancellationToken token = default)
    {
        var dto = await context.Profiles
            .SingleAsync(x => x.Id == profile.Id, token);

        var updated = profile.ToEntityFramework();
        
        dto.Level = updated.Level;
        dto.CompletedChapters = updated.CompletedChapters;
        dto.CurrentChapterId = updated.CurrentChapterId;
        dto.CurrentPageIndex = updated.CurrentPageIndex;
        dto.ValidationResults = updated.ValidationResults;
        
        context.Profiles
            .Update(dto);
        
        await context
            .SaveChangesAsync(token);
    }

    public async Task<IEnumerable<Profile>> FindByUser(string userId, CancellationToken token = default)
    {
        return await context.Profiles
            .Where(x => x.UserId == userId)
            .Select(x => x.ToDomain())
            .ToListAsync(token);
    }
}