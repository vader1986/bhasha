using System.Collections.Immutable;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Pages.Student;
using Orleans;

namespace Bhasha.Web.Grains;

public class SummaryGrain : Grain, ISummaryGrain
{
    private readonly IChapterLookup _chapterLookup;
    private readonly ITranslationProvider _translationProvider;
    private readonly IList<Summary> _summaries = new List<Summary>();

    public SummaryGrain(IChapterLookup chapterLookup, ITranslationProvider translationProvider)
    {
        _chapterLookup = chapterLookup;
        _translationProvider = translationProvider;
    }

    public override async Task OnActivateAsync()
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

            translations[expressionId] = translation.Native;
            return translation.Native;
        }

        await foreach (var chapter in _chapterLookup.GetChapters(key.Level))
        {
            var name = await Translate(chapter.NameId);
            var description = await Translate(chapter.DescriptionId);

            _summaries.Add(new Summary(chapter.Id, name, description));
        }

        await base.OnActivateAsync();
    }

    public ValueTask<ImmutableList<Summary>> GetSummaries()
    {
        return new ValueTask<ImmutableList<Summary>>(_summaries.ToImmutableList());
    }
}

