using Bhasha.Domain;
using Bhasha.Domain.Interfaces;

namespace Bhasha.Services;

public interface IChapterProvider
{
    Task<DisplayedChapter> Load(ChapterKey key, CancellationToken token = default);
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
    
    public async Task<DisplayedChapter> Load(ChapterKey key, CancellationToken token)
    {
        var chapter = await _chapterRepository.FindById(key.ChapterId, token);
        if (chapter == null) throw new InvalidOperationException($"Chapter with ID {key.ChapterId} not found");

        var displayedPages = new List<DisplayedPage>(capacity: chapter.Pages.Length);
        foreach (var page in chapter.Pages)
        {
            displayedPages.Add(await _pageFactory.Create(chapter, page, key.ProfileKey));
        }
        
        var name = await _translationProvider.Find(chapter.Name.Id, key.ProfileKey.Native, token)
                   ?? throw new InvalidOperationException($"Translation for {chapter.Name.Id} to {key.ProfileKey.Native} not found");

        var description = await _translationProvider.Find(chapter.Description.Id, key.ProfileKey.Native, token)
            ?? throw new InvalidOperationException($"Translation for {chapter.Description.Id} to {key.ProfileKey.Native} not found");

        return new DisplayedChapter(key.ChapterId, name.Text, description.Text, displayedPages.ToArray(), chapter.ResourceId);
    }
}