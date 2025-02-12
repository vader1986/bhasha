using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Profile = Bhasha.Domain.Profile;

namespace Bhasha.Services;

public interface IStudyingService
{
    Task<Profile> GetProfile(ProfileKey key);
    Task<IReadOnlyList<Profile>> GetProfiles(string userId);
    Task<Profile> CreateProfile(ProfileKey key);
    Task DeleteProfile(ProfileKey key);
    Task<DisplayedChapter?> GetCurrentChapter(ProfileKey key);
    Task<IReadOnlyList<DisplayedSummary>> GetSummaries(ProfileKey key);
    Task<DisplayedChapter> SelectChapter(ChapterKey key);
    Task<Validation> Submit(ValidationInput input);
}

public class StudyingService(
    IProfileRepository repository,
    IValidator validator,
    IChapterSummariesProvider summariesProvider,
    IChapterProvider chapterProvider) : IStudyingService
{
    private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

    private async Task<T> RunWithProfile<T>(ProfileKey key, Func<Profile, Task<T>> handle)
    {
        if (_cache.TryGetValue<Profile>(key, out var profile) is false)
        {
            foreach (var userProfile in await repository.FindByUser(key.UserId))
            {
                if (userProfile.Key == key)
                    profile = userProfile;

                _cache.Set(userProfile.Key, userProfile);
            }
        }

        if (profile is null)
        {
            throw new InvalidOperationException($"profile {key} not found");
        }

        return await handle(profile);
    }
    
    public async Task<Profile> GetProfile(ProfileKey key)
    {
        return await RunWithProfile(key, Task.FromResult);
    }

    public async Task<IReadOnlyList<Profile>> GetProfiles(string userId)
    {
        var result = new List<Profile>();
        
        foreach (var profile in await repository.FindByUser(userId))
        {
            _cache.Set(profile.Key, profile);
            
            result.Add(profile);
        }

        return result;
    }

    public async Task<Profile> CreateProfile(ProfileKey key)
    {
        if (key.Native == key.Target)
            throw new ArgumentException("Native and target language must differ", nameof(key));

        if (!Language.Supported.ContainsKey(key.Native))
            throw new ArgumentException("Native language is not supported", nameof(key));

        if (!Language.Supported.ContainsKey(key.Target))
            throw new ArgumentException("Target language is not supported", nameof(key));

        var profile = await repository.Add(Profile.Create(key));

        _cache.Set(key, profile);

        return profile;
    }

    public async Task DeleteProfile(ProfileKey key)
    {
        await repository.Delete(key);
        _cache.Remove(key);
    }

    public async Task<DisplayedChapter?> GetCurrentChapter(ProfileKey key)
    {
        return await RunWithProfile(key, async profile =>
        {
            var chapterId = profile.CurrentChapter?.ChapterId;
            if (chapterId is null)
                return null;

            var chapterKey = new ChapterKey(chapterId.Value, key);
            return await chapterProvider.Load(chapterKey);
        });
    }

    public async Task<IReadOnlyList<DisplayedSummary>> GetSummaries(ProfileKey key)
    {
        return await RunWithProfile(key, async profile =>
        {
            var summaries = await summariesProvider.GetSummaries(profile.Level, key.Native);
            
            return summaries
                .Select(summary => new DisplayedSummary(
                    summary.ChapterId,
                    summary.Name,
                    summary.Description,
                    profile.CompletedChapters.Contains(summary.ChapterId)))
                .ToList();
        });
    }

    public async Task<DisplayedChapter> SelectChapter(ChapterKey key)
    {
        return await RunWithProfile(key.ProfileKey, async profile =>
        {
            var chapter = await chapterProvider.Load(key);
            var updatedProfile = profile with
            {
                CurrentChapter = new ChapterSelection(
                    ChapterId: chapter.Id, 
                    PageIndex: 0, 
                    CorrectAnswers: Enumerable
                        .Range(0, chapter.Pages.Length)
                        .Select(_ => (byte)0).ToArray())
            };

            await repository.Update(updatedProfile);

            _cache.Set(updatedProfile.Key, updatedProfile);
            
            return chapter;
        });
    }

    public async Task<Validation> Submit(ValidationInput input)
    {
        return await RunWithProfile(input.Key, async profile =>
        {
            var validation = await validator.Validate(input);

            var chapter = profile.CurrentChapter;
            
            if (chapter is null)
                return validation;
            
            if (validation.Result == ValidationResult.Correct)
            {
                chapter.CorrectAnswers[chapter.PageIndex]++;
            }
            
            var nextPageIndex = GetNextPageIndex(chapter);

            if (nextPageIndex is null)
            {
                var completedChapters = profile.CompletedChapters
                    .Append(chapter.ChapterId)
                    .Distinct().ToArray();

                profile = profile with
                {
                    CompletedChapters = completedChapters,
                    CurrentChapter = null,
                    Level = completedChapters.Length / 5 + 1
                };
            }
            else
            {
                profile = profile with
                {
                    CurrentChapter = chapter with { PageIndex = nextPageIndex.Value }
                };
            }
            
            await repository.Update(profile);

            _cache.Set(profile.Key, profile);

            return validation;
            
            int? GetNextPageIndex(ChapterSelection selection)
            {
                var pages = selection.CorrectAnswers.Length;

                for (var i = 0; i < pages; i++)
                {
                    var index = (selection.PageIndex + 1 + i) % pages;
                    if (selection.CorrectAnswers[index] < 3)
                    {
                        return index;
                    }
                }

                return null;
            }
        });
    }
}