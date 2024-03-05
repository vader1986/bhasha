using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bhasha.Infrastructure.EntityFramework;

public class EntityFrameworkTranslationRepository(AppDbContext context) : ITranslationRepository
{
    public async Task<Translation?> Find(int expressionId, string language, CancellationToken token = default)
    {
        var row = await context.Translations
            .FirstOrDefaultAsync(x => x.Language == language && x.Expression.Id == expressionId, token);

        if (row is null)
            return null;

        return Converter
            .Convert(row);
    }

    public async Task<Translation?> Find(string text, string language, CancellationToken token = default)
    {
        var row = await context.Translations
            .FirstOrDefaultAsync(x => x.Language == language && x.Text == text, token);

        if (row is null)
            return null;

        await context
            .Entry(row)
            .Reference(x => x.Expression)
            .LoadAsync(token);
        
        return Converter
            .Convert(row);
    }

    public async Task<Translation> AddOrUpdate(Translation translation, CancellationToken token = default)
    {
        var row = await context.Translations
            .Include(x => x.Expression)
            .FirstOrDefaultAsync(
                x => x.Expression.Id == translation.Expression.Id && 
                     x.Language == translation.Language, token);

        if (row is not null)
        {
            var updatedTranslation = Converter
                .Convert(translation);

            row.Text = updatedTranslation.Text; 
            
            var result = context.Translations
                .Update(row);
            
            await context
                .SaveChangesAsync(token);

            return Converter
                .Convert(result.Entity);
        }
        else
        {
            var newTranslation = Converter
                .Convert(translation);

            var expression = await context.Expressions
                .FirstAsync(x => x.Id == translation.Expression.Id, token);

            newTranslation.Expression = expression;
            
            var result = await context.Translations
                .AddAsync(newTranslation, token);
            
            await context
                .SaveChangesAsync(token);

            return Converter
                .Convert(result.Entity);
        }
    }

    public async Task<Translation> Get(int translationId, CancellationToken token = default)
    {
        return Converter
            .Convert(await context.Translations
            .FirstAsync(x => x.Id == translationId, token));
    }
}