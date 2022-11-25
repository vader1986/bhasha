﻿using System.Collections.Immutable;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Orleans;

namespace Bhasha.Web.Grains;

public class UserGrain : Grain, IUserGrain
{
    private readonly IRepository<Profile> _profileRepository;
    private readonly IDictionary<LangKey, Profile> _profiles;

    public UserGrain(IRepository<Profile> profileRepository)
	{
        _profileRepository = profileRepository;
        _profiles = new Dictionary<LangKey, Profile>();
    }

    public override async Task OnActivateAsync()
    {
        var userId = this.GetPrimaryKeyString();
        var profiles = await _profileRepository
            .Find(profile => profile.Key.UserId == userId);

        foreach (var profile in profiles)
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
        var chapterId = profile.ChapterId;

        if (chapterId == null)
        {
            return null;
        }

        var chapterKey = new ChapterKey(chapterId.Value, langId);
        var chapterGrain = GrainFactory
            .GetGrain<IDisplayChapterGrain>(chapterKey.ToString());

        return await chapterGrain.Display();
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
}

