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

        var updatedProfile = profile with
        {
            
            ChapterId = chapterId,
            PageIndex = 0,
            Pages = Enumerable.Repeat(ValidationResultType.Wrong, chapter.Pages.Length).ToArray()
        };

        var updated = await _profiles.Update(profileId, updatedProfile);

        if (!updated)
            throw new InvalidOperationException($"failed to updpated profile {profileId}");

        return updatedProfile;
    }

    public async Task<Profile> Update(Profile profile, ValidationResult result)
    {
        var chapterId = profile.ChapterId;
        if (chapterId == null)
            throw new ArgumentOutOfRangeException($"no chapter selected for profile {profile.Id}");

        var chapter = await _chapters.Get(chapterId.Value);
        if (chapter == null)
            throw new ArgumentOutOfRangeException($"chapter {chapterId} not found");

        profile.Pages[profile.PageIndex] = result.Result;

        if (profile.Pages.All(x => x == ValidationResultType.Correct))
        {
            var completedChapters = profile.CompletedChapters.Append(chapter.Id).ToArray();
            profile = profile with
            {
                ChapterId = null,
                CompletedChapters = completedChapters,
                Pages = Array.Empty<ValidationResultType>(),
                Level = completedChapters.Length % 5 + 1 // TODO - improve level calculation
            };
        }
        else
        {
            var nextPage = profile.Pages
                    .Select((status, index) => (status, index))
                    .FirstOrDefault(x => x.index != profile.PageIndex && x.status != ValidationResultType.Correct);

            if (nextPage != default)
            {
                profile = profile with { PageIndex = nextPage.index };
            }
        }

        var updated = await _profiles.Update(profile.Id, profile);

        if (!updated)
            throw new InvalidOperationException($"failed to updated profile {profile.Id}");

        return profile;
    }
}