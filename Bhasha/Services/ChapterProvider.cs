using Bhasha.Domain;
using Bhasha.Domain.Interfaces;

namespace Bhasha.Services;

public interface IChapterProvider
{
    Task<DisplayedChapter> Load(ChapterKey key, CancellationToken token = default);
}

public class ChapterProvider(
    IChapterRepository chapterRepository,
    ITranslationProvider translationProvider,
    IPageFactory pageFactory) : IChapterProvider
{
    public async Task<DisplayedChapter> Load(ChapterKey key, CancellationToken token)
    {
        var chapter = await chapterRepository.FindById(key.ChapterId, token);
        if (chapter == null) throw new InvalidOperationException($"Chapter with ID {key.ChapterId} not found");

        var displayedPages = new List<DisplayedPage>(capacity: chapter.Pages.Length);
        foreach (var page in chapter.Pages)
        {
            displayedPages.Add(await pageFactory.Create(chapter, page, key.ProfileKey));
        }
        
        var name = await translationProvider.Find(chapter.Name.Id, key.ProfileKey.Native, token)
                   ?? throw new InvalidOperationException($"Translation for {chapter.Name.Id} to {key.ProfileKey.Native} not found");

        var description = await translationProvider.Find(chapter.Description.Id, key.ProfileKey.Native, token)
            ?? throw new InvalidOperationException($"Translation for {chapter.Description.Id} to {key.ProfileKey.Native} not found");

        return new DisplayedChapter(key.ChapterId, name.Text, description.Text, displayedPages.ToArray(), chapter.ResourceId);
    }
}