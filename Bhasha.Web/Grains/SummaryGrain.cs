using System.Collections.Immutable;
using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Interfaces;

namespace Bhasha.Web.Grains;

public interface ISummaryGrain : IGrainWithStringKey
{
    ValueTask<ImmutableList<Summary>> GetSummaries();
}

public class SummaryGrain : Grain, ISummaryGrain
{
    private readonly IChapterRepository _chapterRepository;
    private readonly ITranslationRepository _translationProvider;
    private readonly IList<Summary> _summaries = new List<Summary>();

    public SummaryGrain(IChapterRepository chapterRepository, ITranslationRepository translationProvider)
    {
        _chapterRepository = chapterRepository;
        _translationProvider = translationProvider;
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
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

        await foreach (var chapter in _chapterRepository.GetChapters(key.Level))
        {
            var name = await Translate(chapter.NameId);
            var description = await Translate(chapter.DescriptionId);

            _summaries.Add(new Summary(chapter.Id, name, description));
        }
    }

    public ValueTask<ImmutableList<Summary>> GetSummaries()
    {
        return new ValueTask<ImmutableList<Summary>>(_summaries.ToImmutableList());
    }
}

