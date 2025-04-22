using Bhasha.Domain;
using Bhasha.Domain.Interfaces;

namespace Bhasha.Services;

public interface IChapterProvider
{
    Task<DisplayedChapter> Load(ChapterKey key, CancellationToken token = default);
}

public sealed class ChapterProvider(
    IChapterRepository chapterRepository,
    ITranslationProvider translationProvider) : IChapterProvider
{
    public async Task<DisplayedChapter> Load(ChapterKey key, CancellationToken token)
    {
        var chapter = await chapterRepository.FindById(key.ChapterId, token);
        if (chapter == null) throw new InvalidOperationException($"Chapter with ID {key.ChapterId} not found");

        var displayedPages = new List<DisplayedPage>(capacity: chapter.Pages.Length);
        foreach (var page in chapter.Pages)
        {
            var word = await translationProvider.Find(page.Id, key.ProfileKey.Native, token)
                ?? throw new InvalidOperationException($"Translation for {page.Id} to {key.ProfileKey.Native} not found");
            
            displayedPages
                .Add(new DisplayedPage(
                    Word: word, 
                    Lead: null,
                    StudyCard: null));
        }
        
        var name = await translationProvider.Find(chapter.Name.Id, key.ProfileKey.Native, token)
                   ?? throw new InvalidOperationException($"Translation for {chapter.Name.Id} to {key.ProfileKey.Native} not found");

        var description = await translationProvider.Find(chapter.Description.Id, key.ProfileKey.Native, token)
            ?? throw new InvalidOperationException($"Translation for {chapter.Description.Id} to {key.ProfileKey.Native} not found");

        return new DisplayedChapter(
            Id: key.ChapterId, 
            Name: name.Text, 
            Description: description.Text,
            Pages: displayedPages.ToArray(), 
            ResourceId: chapter.ResourceId,
            StudyCards: chapter.StudyCards
                .Where(studyCard => studyCard.Language == key.ProfileKey.Target)
                .ToArray());
    }
}