using Bhasha.Domain;
using Bhasha.Domain.Interfaces;

namespace Bhasha.Services;

public class ChapterSummariesProvider : IChapterSummariesProvider
{
    private readonly IChapterRepository _chapterRepository;
    private readonly ITranslationRepository _translationProvider;

    public ChapterSummariesProvider(IChapterRepository chapterRepository, ITranslationRepository translationProvider)
    {
        _chapterRepository = chapterRepository;
        _translationProvider = translationProvider;
    }

    public async Task<IReadOnlyList<Summary>> GetSummaries(int level, LangKey languages)
    {
        var summaries = new List<Summary>();
        var translations = new Dictionary<Guid, string>();
        var native = languages.Native;

        async Task<string> Translate(Guid expressionId)
        {
            if (translations.TryGetValue(expressionId, out var name))
            {
                return name;
            }

            var translation = await _translationProvider.Find(expressionId, native);
            if (translation == null)
            {
                throw new InvalidOperationException($"Translation for {expressionId} to {native} not found");
            }

            translations[expressionId] = translation.Text;
            return translation.Text;
        }

        await foreach (var chapter in _chapterRepository.FindByLevel(level))
        {
            var name = await Translate(chapter.NameId);
            var description = await Translate(chapter.DescriptionId);

            summaries.Add(new Summary(chapter.Id, name, description));
        }

        return summaries;
    }
}

