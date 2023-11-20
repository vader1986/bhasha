using Bhasha.Domain.Extensions;
using Bhasha.Domain.Interfaces;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Bhasha.Services;

public interface IStudyingService
{
    Task<Profile> GetProfile(ProfileKey key);
    Task<IReadOnlyList<Profile>> GetProfiles(string userId);
    Task<Profile> CreateProfile(ProfileKey key);
    Task<DisplayedChapter?> GetCurrentChapter(ProfileKey key);
    Task<IReadOnlyList<DisplayedSummary>> GetSummaries(ProfileKey key);
    Task<DisplayedChapter> SelectChapter(ChapterKey key);
    Task<Validation> Submit(ValidationInput input);
}

public class StudyingService : IStudyingService
{
    private readonly IProfileRepository _repository;
    private readonly IValidator _validator;
    private readonly IChapterSummariesProvider _summariesProvider;
    private readonly IChapterProvider _chapterProvider;
    private readonly IMemoryCache _cache;
    
    public StudyingService(IProfileRepository repository, IValidator validator, IChapterSummariesProvider summariesProvider, IChapterProvider chapterProvider)
    {
        _repository = repository;
        _validator = validator;
        _summariesProvider = summariesProvider;
        _chapterProvider = chapterProvider;
        _cache = new MemoryCache(new MemoryCacheOptions());
    }
    
    private async Task<T> RunWithProfile<T>(ProfileKey key, Func<Profile, Task<T>> handle)
    {
        if (_cache.TryGetValue<Profile>(key, out var profile) is false)
        {
            await foreach (var userProfile in _repository.FindByUser(key.UserId))
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
        
        await foreach (var profile in _repository.FindByUser(userId))
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

        var profile = await _repository.Add(Profile.Create(key));

        _cache.Set(key, profile);

        return profile;
    }

    public async Task<DisplayedChapter?> GetCurrentChapter(ProfileKey key)
    {
        return await RunWithProfile(key, async profile =>
        {
            var chapterId = profile.CurrentChapter?.ChapterId;
            if (chapterId is null)
                return null;

            var chapterKey = new ChapterKey(chapterId.Value, key);
            return await _chapterProvider.Load(chapterKey);
        });
    }

    public async Task<IReadOnlyList<DisplayedSummary>> GetSummaries(ProfileKey key)
    {
        return await RunWithProfile(key, async profile =>
        {
            var summaries = await _summariesProvider.GetSummaries(profile.Level, key);
            
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
            var chapter = await _chapterProvider.Load(key);
            var updatedProfile = profile.Select(chapter);

            await _repository.Update(updatedProfile);

            _cache.Set(updatedProfile.Key, updatedProfile);
            
            return chapter;
        });
    }

    public async Task<Validation> Submit(ValidationInput input)
    {
        return await RunWithProfile(input.Key, async profile =>
        {
            var validation = await _validator.Validate(input);
            var updatedProfile = profile.Submit(validation.Result);

            await _repository.Update(updatedProfile);

            _cache.Set(updatedProfile.Key, updatedProfile);

            return validation;
        });
    }
}