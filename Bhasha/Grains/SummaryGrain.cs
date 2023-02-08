using System.Collections.Immutable;
using Bhasha.Domain;
using Bhasha.Domain.Interfaces;

namespace Bhasha.Grains;

public interface ISummaryGrain : IGrainWithStringKey
{
    Task<ImmutableList<Summary>> GetSummaries();
}

public class SummaryGrain : Grain, ISummaryGrain
{
    private readonly IChapterRepository _chapterRepository;
    private readonly ITranslationRepository _translationProvider;
    private IList<Summary>? _summaries;

    public SummaryGrain(IChapterRepository chapterRepository, ITranslationRepository translationProvider)
    {
        _chapterRepository = chapterRepository;
        _translationProvider = translationProvider;
    }

    public async Task<ImmutableList<Summary>> GetSummaries()
    {
        if (_summaries == null)
        {
            var summaries = new List<Summary>();
            var key = SummaryCollectionKey.Parse(this.GetPrimaryKeyString());
            var translations = new Dictionary<Guid, string>();
            var native = key.LangId.Native;

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

            await foreach (var chapter in _chapterRepository.FindByLevel(key.Level))
            {
                var name = await Translate(chapter.NameId);
                var description = await Translate(chapter.DescriptionId);

                summaries.Add(new Summary(chapter.Id, name, description));
            }

            _summaries = summaries;
        }

        return _summaries.ToImmutableList();
    }
}

