using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Orleans;

namespace Bhasha.Web.Grains;

public interface IDisplayChapterGrain : IGrainWithStringKey
{
    ValueTask<DisplayedChapter> Display();
}

public class DisplayChapterGrain : Grain, IDisplayChapterGrain
{
    private readonly IRepository<Chapter> _chapterRepository;
    private readonly ITranslationProvider _translationProvider;
    private readonly IAsyncFactory<Page, LangKey, DisplayedPage> _pageFactory;

    private DisplayedChapter? _state;
    
    public DisplayChapterGrain(IRepository<Chapter> chapterRepository, ITranslationProvider translationProvider, IAsyncFactory<Page, LangKey, DisplayedPage> pageFactory)
    {
        _chapterRepository = chapterRepository;
        _translationProvider = translationProvider;
        _pageFactory = pageFactory;
    }

    public override async Task OnActivateAsync()
    {
        var key = ChapterKey.From(this.GetPrimaryKeyString());

        var chapter = await _chapterRepository.Get(key.ChapterId);
        if (chapter == null) throw new ArgumentOutOfRangeException(nameof(key.ChapterId));

        var pages = await Task.WhenAll(chapter.Pages.Select(
            async page => await _pageFactory.CreateAsync(page, key.LangId)));

        var name = await _translationProvider.Find(chapter.NameId, key.LangId.Native)
            ?? throw new InvalidOperationException($"Translation for {chapter.NameId} to {key.LangId.Native} not found");

        var description = await _translationProvider.Find(chapter.DescriptionId, key.LangId.Native)
            ?? throw new InvalidOperationException($"Translation for {chapter.DescriptionId} to {key.LangId.Native} not found");

        _state = new DisplayedChapter(key.ChapterId, name.Native, description.Native, pages, chapter.ResourceId);

        await base.OnActivateAsync();
    }

    public ValueTask<DisplayedChapter> Display()
    {
        if (_state == null)
        {
            throw new InvalidOperationException("chapter is not setup");
        }

        return new ValueTask<DisplayedChapter>(_state);
    }
}