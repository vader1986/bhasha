using Bhasha.Domain.Interfaces;
using Bhasha.Infrastructure.EntityFramework.Dtos;
using Microsoft.EntityFrameworkCore;
using Chapter = Bhasha.Domain.Chapter;

namespace Bhasha.Infrastructure.EntityFramework;

public sealed class EntityFrameworkChapterRepository(AppDbContext context) : IChapterRepository
{
    private async Task LoadDependencies(ChapterDto dto, Chapter chapter, CancellationToken token)
    {
        dto.Name = await context.Expressions
            .SingleAsync(x => x.Id == chapter.Name.Id, token);
            
        dto.Description = await context.Expressions
            .SingleAsync(x => x.Id == chapter.Description.Id, token);

        var pages = new List<ExpressionDto>();

        foreach (var page in chapter.Pages)
        {
            pages.Add(await context.Expressions.SingleAsync(x => x.Id == page.Id, token));
        }
        
        dto.Expressions = pages;
    }
    
    public async Task<Chapter> AddOrUpdate(Chapter chapter, CancellationToken token)
    {
        var row = await context.Chapters
            .FirstOrDefaultAsync(x => x.Id == chapter.Id, token);

        if (row is not null)
        {
            await LoadDependencies(row, chapter, token);
            
            var result = context.Chapters.Update(row);

            await context.SaveChangesAsync(token);

            return result.Entity.ToDomain();
        }
        else
        {
            var newChapter = chapter.ToEntityFramework();

            await LoadDependencies(newChapter, chapter, token);
            
            var result = await context.Chapters.AddAsync(newChapter, token);

            await context.SaveChangesAsync(token);
        
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

        return row?.ToDomain();
    }

    public async Task<IEnumerable<Chapter>> FindByLevel(int level, CancellationToken token)
    {
        return await context.Chapters
            .Include(x => x.Name)
            .Include(x => x.Description)
            .Include(x => x.Expressions)
            .Where(x => x.RequiredLevel == level)
            .Select(x => x.ToDomain())
            .ToListAsync(token);
    }

    public async Task<IEnumerable<Chapter>> FindAll(int offset, int batchSize, CancellationToken token)
    {
        return await context.Chapters
            .Skip(offset)
            .Take(batchSize)
            .Include(x => x.Name)
            .Include(x => x.Description)
            .Include(x => x.Expressions)
            .Select(x => x.ToDomain())
            .ToListAsync(token);
    }

    public async Task Delete(int chapterId, CancellationToken token)
    {
        var chapter = await context.Chapters
            .FirstOrDefaultAsync(x => x.Id == chapterId, token);
        
        if (chapter is null)
            return;
        
        context.Chapters.Remove(chapter);
        
        await context.SaveChangesAsync(token);
    }
}