using Bhasha.Domain.Interfaces;
using Bhasha.Infrastructure.EntityFramework.Dtos;
using Microsoft.EntityFrameworkCore;
using Chapter = Bhasha.Domain.Chapter;

namespace Bhasha.Infrastructure.EntityFramework;

public class EntityFrameworkChapterRepository(AppDbContext context) : IChapterRepository
{
    private async Task LoadDependencies(ChapterDto dto, Chapter chapter, CancellationToken token)
    {
        dto.Name = await context.Expressions
            .SingleAsync(x => x.Id == chapter.Name.Id, token);
            
        dto.Description = await context.Expressions
            .SingleAsync(x => x.Id == chapter.Description.Id, token);

        var pageIds = chapter.Pages
            .Select(x => x.Id);
            
        dto.Expressions = await context.Expressions
            .Where(x => pageIds.Contains(x.Id))
            .ToArrayAsync(token);
    }
    
    public async Task<Chapter> AddOrUpdate(Chapter chapter, CancellationToken token)
    {
        var row = await context.Chapters
            .FirstOrDefaultAsync(x => x.Id == chapter.Id, token);

        if (row is not null)
        {
            context.Chapters
                .Remove(row);
            
            var updatedChapter = Converter
                .Convert(chapter);

            await LoadDependencies(updatedChapter, chapter, token);
            
            var result = await context.Chapters
                .AddAsync(updatedChapter, token);

            return Converter
                .Convert(result.Entity);
        }
        else
        {
            var newChapter = Converter.Convert(chapter);

            await LoadDependencies(newChapter, chapter, token);
            
            var result = await context.Chapters
                .AddAsync(newChapter, token);

            await context
                .SaveChangesAsync(token);
        
            return chapter with
            {
                Id = result.Entity.Id
            };   
        }
    }

    public async Task<Chapter?> FindById(int chapterId, CancellationToken token)
    {
        var row = await context.Chapters
            .Include(x => x.Name)
            .Include(x => x.Description)
            .Include(x => x.Expressions)
            .FirstOrDefaultAsync(x => x.Id == chapterId, token);

        return row is null ? null : Converter.Convert(row);
    }

    public async Task<IEnumerable<Chapter>> FindByLevel(int level, CancellationToken token)
    {
        return await context.Chapters
            .Include(x => x.Name)
            .Include(x => x.Description)
            .Include(x => x.Expressions)
            .Where(x => x.RequiredLevel == level)
            .Select(x => Converter.Convert(x))
            .ToListAsync(token);
    }
}