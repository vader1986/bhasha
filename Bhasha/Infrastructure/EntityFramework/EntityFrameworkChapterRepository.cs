using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bhasha.Infrastructure.EntityFramework;

public class EntityFrameworkChapterRepository(AppDbContext context) : IChapterRepository
{
    public async Task<Chapter> AddOrReplace(Chapter chapter, CancellationToken token)
    {
        var row = await context.Chapters
            .FirstOrDefaultAsync(x => x.Id == chapter.Id, token);
        
        if (row is not null) 
            context.Chapters.Remove(row);
        
        var result = await context.Chapters
            .AddAsync(Converter.Convert(chapter), token);

        await context
            .SaveChangesAsync(token);
        
        return chapter with
        {
            Id = result.Entity.Id
        };
    }

    public async Task<Chapter?> FindById(int chapterId, CancellationToken token)
    {
        var row = await context.Chapters
            .FirstOrDefaultAsync(x => x.Id == chapterId, token);

        return row is null ? null : Converter.Convert(row);
    }

    public IAsyncEnumerable<Chapter> FindByLevel(int level, CancellationToken token)
    {
        return context.Chapters
            .Where(x => x.RequiredLevel == level)
            .Select(x => Converter.Convert(x))
            .ToAsyncEnumerable();
    }
}