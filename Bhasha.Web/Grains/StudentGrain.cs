using System.Collections.Immutable;
using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Interfaces;
using Orleans.Streams;

namespace Bhasha.Web.Grains;

public interface IStudentGrain : IGrainWithStringKey
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
    Task<ImmutableList<DisplayedSummary>> GetSummaries(LangKey langId);

    /// <summary>
    /// User selects a chapter for the selected languages to continue learn a
    /// set of new words. 
    /// </summary>
    /// <param name="chapterId">Unique identifier of the chapter.</param>
    /// <param name="langId">Native language of the user and target language to learn.</param>
    /// <returns>The next chapter to be displayed to the user.</returns>
    Task<DisplayedChapter> SelectChapter(Guid chapterId, LangKey langId);

    /// <summary>
    /// Gets a list of profiles created for the user.
    /// </summary>
    /// <returns>An immutable list of all user profiles.</returns>
    Task<ImmutableList<Profile>> GetProfiles();

    /// <summary>
    /// Submit user input for validating. Updates the corresponding user profile
    /// after validation of the input. 
    /// </summary>
    /// <param name="input">Input for validation.</param>
    /// <returns></returns>
    Task<Validation> Submit(ValidationInput input);
}

public class StudentGrain : Grain, IStudentGrain
{
    private readonly IProfileRepository _repository;
    private readonly IValidator _validator;
    private readonly IDictionary<LangKey, Profile> _profiles;
    private IAsyncStream<Profile>? _stream;


    public StudentGrain(IProfileRepository repository, IValidator validator)
	{
        _repository = repository;
        _validator = validator;
        _profiles = new Dictionary<LangKey, Profile>();
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        var userId = this.GetPrimaryKeyString();
        
        await foreach (var profile in _repository.FindByUser(userId))
        {
            _profiles[profile.Key.LangId] = profile;
        }

        var streamProvider = this.GetStreamProvider(Orleans.StreamProvider);
        var stream = streamProvider.GetStream<Profile>(Orleans.Streams.UserProfile);

        _stream = stream;
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
        var chapterGrain = GrainFactory.GetGrain<IDisplayChapterGrain>(chapterKey.ToString());

        return await chapterGrain.Display();
    }

    public async Task<ImmutableList<DisplayedSummary>> GetSummaries(LangKey langId)
    {
        var profile = await GetProfile(langId);

        var summariesKey = new SummaryCollectionKey(profile.Level, langId);
        var summaryGrain = GrainFactory
            .GetGrain<ISummaryGrain>(summariesKey.ToString());

        var summaries = await summaryGrain.GetSummaries();

        return summaries
            .Select(summary => new DisplayedSummary(
                summary.ChapterId,
                summary.Name,
                summary.Description,
                profile.CompletedChapters.Contains(summary.ChapterId)))
            .ToImmutableList();
    }

    public async Task<DisplayedChapter> SelectChapter(Guid chapterId, LangKey langId)
    {
        if (!_profiles.TryGetValue(langId, out var profile))
            throw new ArgumentException($"Profile for {langId} not found", nameof(langId));

        var chapterKey = new ChapterKey(chapterId, langId);
        var chapterGrain = GrainFactory.GetGrain<IDisplayChapterGrain>(chapterKey.ToString());
        var chapter = await chapterGrain.Display();

        var defaultPageIndex = 0;
        var defaultPages = Enumerable
            .Range(0, chapter.Pages.Length)
            .Select(_ => ValidationResult.Wrong).ToArray();

        profile = profile with
        {
            CurrentChapter = new ChapterSelection(chapterId, defaultPageIndex, defaultPages)
        };

        await _repository.Update(profile);

        _profiles[langId] = profile;

        return chapter;
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

    private static int? GetNextPageIndex(ChapterSelection selection)
    {
        var pages = selection.Pages.Length;

        for (var i = 0; i < pages; i++)
        {
            var index = (selection.PageIndex + 1 + i) % pages;
            if (selection.Pages[index] != ValidationResult.Correct)
            {
                return index;
            }
        }

        return null;
    }

    public async Task<Validation> Submit(ValidationInput input)
    {
        var key = input.Languages;

        if (!_profiles.TryGetValue(key, out var profile))
        {
            throw new ArgumentException($"User profile for inputs {key} does not exist", nameof(input));
        }

        var validation = await _validator.Validate(input);
        var chapter = profile.CurrentChapter ??
            throw new InvalidOperationException($"No chapter selected for user profile {key}");

        chapter.Pages[chapter.PageIndex] = validation.Result;

        var nextPageIndex = GetNextPageIndex(chapter);

        if (nextPageIndex == null)
        {
            var completedChapters = profile.CompletedChapters
                    .Append(chapter.ChapterId)
                    .Distinct()
                    .ToArray();

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

        _profiles[key] = profile;

        await _repository.Update(profile);

        if (_stream != null)
        {
            await _stream.OnNextAsync(profile);
        }

        return validation;
    }
}

