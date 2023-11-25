using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bhasha.Infrastructure.EntityFramework;

public class EntityFrameworkTranslationRepository(AppDbContext context) : ITranslationRepository
{
    public async Task<Translation?> Find(int expressionId, Language language, CancellationToken token = default)
    {
        var row = await context.Translations
            .FirstOrDefaultAsync(x => x.Language == language && x.Expression.Id == expressionId, token);

        if (row is null)
            return null;

        return Converter
            .Convert(row);
    }

    public async Task<Translation?> Find(string text, Language language, CancellationToken token = default)
    {
        var row = await context.Translations
            .FirstOrDefaultAsync(x => x.Language == language && x.Text == text, token);

        if (row is null)
            return null;

        return Converter
            .Convert(row);
    }

    public async Task AddOrReplace(Translation translation, CancellationToken token = default)
    {
        var row = await context.Translations
            .FirstOrDefaultAsync(x => x.Id == translation.Id, token);

        if (row is not null)
            context.Translations.Remove(row);

        await context.Translations
            .AddAsync(Converter.Convert(translation), token);

        await context
            .SaveChangesAsync(token);
    }

    public async Task<Translation> Get(int translationId, CancellationToken token = default)
    {
        return Converter
            .Convert(await context.Translations
            .FirstAsync(x => x.Id == translationId, token));
    }
}