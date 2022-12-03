using System.Collections.Immutable;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Orleans;

namespace Bhasha.Web.Grains;

public interface IUserGrain : IGrainWithStringKey
{
    /// <summary>
    /// Gets the profile for the specified language combination for the current
    /// user. 
    /// </summary>
    /// <param name="langId">Language combination of the profile</param>
    /// <returns>User profile for the specified language combination.</returns>
    /// <exception cref="InvalidOperationException">profile does not exist</exception>
    ValueTask<Profile> GetProfile(LangKey langId);

    /// <summary>
    /// Creates a new user profile for the specified languages.
    /// </summary>
    /// <param name="langId">Native and target language for the new profile.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Profile for the specified languages
    /// already exists or invalid combination of specified language</exception>
    Task<Profile> CreateProfile(LangKey langId);

    /// <summary>
    /// Gets the current chapter of the profile. If no chapter is
    /// selected <c>null</c> is returned. 
    /// </summary>
    /// <param name="langId">Language combination of the profile</param>
    /// <returns>Current chapter or <c>null</c> if no chapter is selected.</returns>
    /// <exception cref="InvalidOperationException">profile does not exist</exception>
    Task<DisplayedChapter?> GetCurrentChapter(LangKey langId);

    /// <summary>
    /// Gets a list of chapter summaries for uncompleted chapters for the
    /// selected user profile. 
    /// </summary>
    /// <param name="langId">Language combination of the profile</param>
    /// <returns>A list of uncompleted chapter summaries.</returns>
    Task<ImmutableList<Summary>> GetSummaries(LangKey langId);

    /// <summary>
    /// Gets a list of profiles created for the user.
    /// </summary>
    /// <returns>An immutable list of all user profiles.</returns>
    Task<ImmutableList<Profile>> GetProfiles();
}

public class UserGrain : Grain, IUserGrain
{
    private readonly IProfileRepository _repository;
    private readonly IDictionary<LangKey, Profile> _profiles;

    public UserGrain(IProfileRepository repository)
	{
        _repository = repository;
        _profiles = new Dictionary<LangKey, Profile>();
    }

    public override async Task OnActivateAsync()
    {
        var userId = this.GetPrimaryKeyString();
        
        await foreach (var profile in _repository.GetProfiles(userId))
        {
            _profiles[profile.Key.LangId] = profile;
        }
    }

    public ValueTask<Profile> GetProfile(LangKey langId)
    {
        if (!_profiles.TryGetValue(langId, out var profile))
        {
            throw new InvalidOperationException("profile does not exist");
        }

        return new ValueTask<Profile>(profile);
    }

    public async Task<DisplayedChapter?> GetCurrentChapter(LangKey langId)
    {
        var profile = await GetProfile(langId);
        var chapterId = profile.CurrentChapter?.ChapterId;

        if (chapterId == null)
        {
            return null;
        }

        var chapterKey = new ChapterKey(chapterId.Value, langId);
        var chapterGrain = GrainFactory
            .GetGrain<IDisplayChapterGrain>(chapterKey.ToString());

        return await chapterGrain
            .Display();
    }

    public async Task<ImmutableList<Summary>> GetSummaries(LangKey langId)
    {
        var profile = await GetProfile(langId);

        var summariesKey = new SummaryCollectionKey(profile.Level, langId);
        var summaryGrain = GrainFactory
            .GetGrain<ISummaryGrain>(summariesKey.ToString());

        var summaries = await summaryGrain.GetSummaries();

        return summaries
            .Where(summary => !profile.CompletedChapters.Contains(summary.ChapterId))
            .ToImmutableList();
    }

    public Task<ImmutableList<Profile>> GetProfiles()
    {
        return Task.FromResult(_profiles.Values.ToImmutableList());
    }

    public async Task<Profile> CreateProfile(LangKey langId)
    {
        if (langId.Native == langId.Target)
            throw new ArgumentException("Native and target language must differ", nameof(langId));

        if (!Language.Supported.ContainsKey(langId.Native))
            throw new ArgumentException("Native language is not supported", nameof(langId));

        if (!Language.Supported.ContainsKey(langId.Target))
            throw new ArgumentException("Target language is not supported", nameof(langId));

        if (_profiles.ContainsKey(langId))
            throw new ArgumentException($"User profile for {langId} already exists", nameof(langId));

        var userId = this.GetPrimaryKeyString();
        var profileKey = new ProfileKey(userId, langId);
        var profile = await _repository.Add(Profile.Empty(profileKey));

        _profiles[langId] = profile;

        return profile;
    }
}

