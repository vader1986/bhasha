using System.Collections.Immutable;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Orleans;

namespace Bhasha.Web.Grains;

public interface ISummaryGrain : IGrainWithStringKey
{
    ValueTask<ImmutableList<Summary>> GetSummaries();
}

public class SummaryGrain : Grain, ISummaryGrain
{
    private readonly IRepository<Chapter> _chapterRepository;
    private readonly ITranslationProvider _translationProvider;
    private readonly IList<Summary> _summaries = new List<Summary>();

    public SummaryGrain(IRepository<Chapter> chapterRepository, ITranslationProvider translationProvider)
    {
        _chapterRepository = chapterRepository;
        _translationProvider = translationProvider;
    }

    public override async Task OnActivateAsync()
    {
        var args = this.GetPrimaryKeyString().Split("-");
        var level = int.Parse(args[0]);
        var language = Language.Parse(args[1]);

        var chapters = await _chapterRepository
            .Find(chapter => chapter.RequiredLevel == level);

        var translationIds = chapters
            .Select(chapter => chapter.DescriptionId)
            .Concat(chapters.Select(chapter => chapter.NameId))
            .Distinct()
            .ToArray();

        var translations = await _translationProvider
            .FindAll(language, translationIds);

        foreach (var chapter in chapters)
        {
            var name = translations[chapter.NameId].Native;
            var description = translations[chapter.DescriptionId].Native;

            _summaries.Add(new Summary(chapter.Id, name, description));
        }

        await base.OnActivateAsync();
    }

    public ValueTask<ImmutableList<Summary>> GetSummaries()
    {
        return new ValueTask<ImmutableList<Summary>>(_summaries.ToImmutableList());
    }
}

