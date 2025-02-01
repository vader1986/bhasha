using Bhasha.Domain;
using Bhasha.Domain.Interfaces;

namespace Bhasha.Services;

public class ChapterSummariesProvider(
    IChapterRepository chapterRepository,
    ITranslationProvider translationProvider) : IChapterSummariesProvider
{
    private async IAsyncEnumerable<Summary> LoadSummaries(IEnumerable<Chapter> chapters, Language language)
    {
        var translations = new Dictionary<int, string>();
        
        foreach (var chapter in chapters)
        {
            var name = await Translate(chapter.Name.Id);
            var description = await Translate(chapter.Description.Id);

            yield return new Summary(chapter.Id, name, description);
        }
        
        async Task<string> Translate(int expressionId)
        {
            if (translations.TryGetValue(expressionId, out var name))
            {
                return name;
            }

            var translation = await translationProvider.Find(expressionId, language);
            if (translation == null)
            {
                throw new InvalidOperationException($"translation for {expressionId} to {language} not found");
            }

            translations[expressionId] = translation.Text;
            return translation.Text;
        }
    }

    public async Task<IReadOnlyList<Summary>> GetSummaries(int level, Language language, CancellationToken token = default)
    {
        var chapters = await chapterRepository.FindByLevel(level, token);
        var summaries = LoadSummaries(chapters, language);

        return await summaries.ToListAsync(token);
    }

    public async Task<IReadOnlyList<Summary>> GetSummaries(int offset, int batchSize, Language language, CancellationToken token = default)
    {
        var chapters = await chapterRepository.FindAll(offset, batchSize, token);
        var summaries = LoadSummaries(chapters, language);

        return await summaries.ToListAsync(token);
    }
}

