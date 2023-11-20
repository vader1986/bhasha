using Bhasha.Domain.Interfaces;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;

namespace Bhasha.Services;

public class ChapterSummariesProvider : IChapterSummariesProvider
{
    private readonly IChapterRepository _chapterRepository;
    private readonly ITranslationRepository _translationRepository;

    public ChapterSummariesProvider(IChapterRepository chapterRepository, ITranslationRepository translationRepository)
    {
        _chapterRepository = chapterRepository;
        _translationRepository = translationRepository;
    }

    public async Task<IReadOnlyList<Summary>> GetSummaries(int level, ProfileKey key)
    {
        var summaries = new List<Summary>();
        var translations = new Dictionary<Guid, string>();
        var native = key.Native;

        await foreach (var chapter in _chapterRepository.FindByLevel(level))
        {
            var name = await Translate(chapter.NameId);
            var description = await Translate(chapter.DescriptionId);

            summaries.Add(new Summary(chapter.Id, name, description));
        }

        return summaries;

        async Task<string> Translate(Guid expressionId)
        {
            if (translations.TryGetValue(expressionId, out var name))
            {
                return name;
            }

            var translation = await _translationRepository.Find(expressionId, native);
            if (translation == null)
            {
                throw new InvalidOperationException($"translation for {expressionId} to {native} not found");
            }

            translations[expressionId] = translation.Text;
            return translation.Text;
        }
    }
}

