using System;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services;

public class ProgressManager : IProgressManager
{
    private readonly IRepository<Chapter> _chapters;
    private readonly IRepository<Profile> _profiles;

    public ProgressManager(IRepository<Chapter> chapters, IRepository<Profile> profiles)
	{
        _chapters = chapters;
        _profiles = profiles;
    }

    public async Task<Profile> StartChapter(Guid profileId, Guid chapterId)
    {
        var profile = await _profiles.Get(profileId);

        if (profile == null)
            throw new ArgumentOutOfRangeException($"profile {profileId} not found");

        var chapter = await _chapters.Get(chapterId);

        if (chapter == null)
            throw new ArgumentOutOfRangeException($"chapter {chapterId} not found");

        var defaultResults = Enumerable.Repeat(ValidationResultType.Wrong, chapter.Pages.Length).ToArray();

        var updatedProfile = profile with
        {
            CurrentChapter = new ChapterSelection(
                ChapterId: chapterId,
                PageIndex: 0,
                Pages: defaultResults)
        };

        var updated = await _profiles.Update(profileId, updatedProfile);

        if (!updated)
            throw new InvalidOperationException($"failed to updpated profile {profileId}");

        return updatedProfile;
    }

    public async Task<Profile> Update(Profile profile, ValidationResult result)
    {
        var currentChapter = profile.CurrentChapter;
        if (currentChapter == null)
            throw new ArgumentOutOfRangeException($"no chapter selected for profile {profile.Id}");

        var chapterId = currentChapter.ChapterId;
        var chapter = await _chapters.Get(chapterId);

        if (chapter == null)
            throw new ArgumentOutOfRangeException($"chapter {chapterId} not found");

        currentChapter.Pages[currentChapter.PageIndex] = result.Result;

        if (currentChapter.Pages.All(x => x == ValidationResultType.Correct))
        {
            var completedChapters = profile.CompletedChapters.Append(chapter.Id).ToArray();
            profile = profile with
            {
                Level = completedChapters.Length % 5 + 1, // TODO - improve level calculation
                CompletedChapters = completedChapters
            };

            currentChapter = null;
        }
        else
        {
            var nextPage = currentChapter.Pages
                    .Select((status, index) => (status, index))
                    .FirstOrDefault(x => x.index != currentChapter.PageIndex &&
                                         x.status != ValidationResultType.Correct);

            if (nextPage != default)
            {
                currentChapter = currentChapter with { PageIndex = nextPage.index };
            }
        }

        var updatedProfile = profile with { CurrentChapter = currentChapter };
        var updated = await _profiles.Update(profile.Id, updatedProfile);

        if (!updated)
            throw new InvalidOperationException($"failed to updated profile {profile.Id}");

        return updatedProfile;
    }
}