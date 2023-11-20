using Bhasha.Domain.Interfaces;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;

namespace Bhasha.Services;

public interface IChapterProvider
{
    Task<DisplayedChapter> Load(ChapterKey key);
}

public class ChapterProvider : IChapterProvider
{
    private readonly IChapterRepository _chapterRepository;
    private readonly ITranslationRepository _translationProvider;
    private readonly IPageFactory _pageFactory;

    public ChapterProvider(IChapterRepository chapterRepository, ITranslationRepository translationProvider, IPageFactory pageFactory)
    {
        _chapterRepository = chapterRepository;
        _translationProvider = translationProvider;
        _pageFactory = pageFactory;
    }
    
    public async Task<DisplayedChapter> Load(ChapterKey key)
    {
        var chapter = await _chapterRepository.FindById(key.ChapterId);
        if (chapter == null) throw new InvalidOperationException($"Chapter with ID {key.ChapterId} not found");

        var pages = await Task.WhenAll(chapter.Pages.Select(
            async page => await _pageFactory.CreateAsync(page, key.ProfileKey)));

        var name = await _translationProvider.Find(chapter.NameId, key.ProfileKey.Native)
                   ?? throw new InvalidOperationException($"Translation for {chapter.NameId} to {key.ProfileKey.Native} not found");

        var description = await _translationProvider.Find(chapter.DescriptionId, key.ProfileKey.Native)
                          ?? throw new InvalidOperationException($"Translation for {chapter.DescriptionId} to {key.ProfileKey.Native} not found");

        return new DisplayedChapter(key.ChapterId, name.Text, description.Text, pages, chapter.ResourceId);
    }
}