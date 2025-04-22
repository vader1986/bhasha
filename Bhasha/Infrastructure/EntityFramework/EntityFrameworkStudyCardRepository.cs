using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bhasha.Infrastructure.EntityFramework;

public sealed class EntityFrameworkStudyCardRepository(AppDbContext context) : IStudyCardRepository
{
    public async Task<StudyCard> GetById(int id, CancellationToken token = default) 
        => (await context.StudyCards
            .FirstAsync(x => x.Id == id, token))
            .ToDomain();

    public async Task Delete(int id, CancellationToken token = default)
    {
        var dto = await context.StudyCards
            .SingleAsync(x => x.Id == id, token);
        
        context.StudyCards.Remove(dto);
        
        await context.SaveChangesAsync(token);
    }

    public async Task<StudyCard> AddOrUpdate(StudyCard studyCard, CancellationToken token = default)
    {
        if (studyCard.Id == 0)
        {
            var result = await context.StudyCards
                .AddAsync(studyCard.ToEntityFramework(), token);
            
            await context.SaveChangesAsync(token);
            
            return result.Entity.ToDomain();
        }
        
        var dto = await context.StudyCards
            .SingleAsync(x => x.Id == studyCard.Id, token);
        
        dto.Name = studyCard.Name;
        dto.Content = studyCard.Content;
        dto.Language = studyCard.Language;
        dto.StudyLanguage = studyCard.StudyLanguage;
        dto.AudioId = studyCard.AudioId;
        
        context.StudyCards.Update(dto);
        
        await context.SaveChangesAsync(token);
        
        return dto.ToDomain();
    }

    public async Task<IEnumerable<StudyCard>> FindByLanguage(string language, string studyLanguage, CancellationToken token = default)
    {
        return await context.StudyCards
            .Where(x => x.Language == language &&
                        x.StudyLanguage == studyLanguage)
            .Select(x => x.ToDomain())
            .ToListAsync(token);
    }
}