using System;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services;

public class ProfileRepository : IProfileRepository
{
    private readonly IRepository<Profile> _repository;

    public ProfileRepository(IRepository<Profile> repository)
	{
        _repository = repository;
    }

    public async Task<Profile> Add(Profile profile)
    {
        return await _repository.Add(profile);
    }

    public async IAsyncEnumerable<Profile> GetProfiles(string userId)
    {
        var profiles = await _repository.Find(profile => profile.Key.UserId == userId);

        foreach (var profile in profiles)
        {
            yield return profile;
        }
    }
}

