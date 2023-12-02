using Bhasha.Domain;
using Bhasha.Domain.Interfaces;

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

    public async Task<IReadOnlyList<Summary>> GetSummaries(int level, ProfileKey key, CancellationToken token = default)
    {
        var summaries = new List<Summary>();
        var translations = new Dictionary<int, string>();
        var native = key.Native;

        foreach (var chapter in await _chapterRepository.FindByLevel(level, token))
        {
            var name = await Translate(chapter.Name.Id);
            var description = await Translate(chapter.Description.Id);

            summaries.Add(new Summary(chapter.Id, name, description));
        }

        return summaries;

        async Task<string> Translate(int expressionId)
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

