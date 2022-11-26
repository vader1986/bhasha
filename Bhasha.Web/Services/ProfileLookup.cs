using System;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services;

public class ProfileLookup : IProfileLookup
{
    private readonly IRepository<Profile> _repository;

    public ProfileLookup(IRepository<Profile> repository)
	{
        _repository = repository;
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

