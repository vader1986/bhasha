using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services;

public class ProfileManager : IProfileManager
{
    private readonly IRepository<Profile> _repository;
    private readonly IFactory<Profile> _factory;

    public ProfileManager(IRepository<Profile> repository, IFactory<Profile> factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<Profile> Create(string userId, Language native, Language target)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));

        if (!native.IsSupported())
            throw new ArgumentException($"Native language {native} is not supported", nameof(native));

        if (!target.IsSupported())
            throw new ArgumentException($"Native language {target} is not supported", nameof(target));

        if (native == target)
            throw new ArgumentException("Native and target language cannot be equal", nameof(native));

        var existingProfile = await _repository.Find(
            x => x.UserId == userId && x.Native == native && x.Target == target);

        if (existingProfile.Any())
            throw new InvalidOperationException($"Profile {native} - {target} for user {userId} already exists");

        var profile = _factory.Create() with { UserId = userId, Native = native, Target = target };
        return await _repository.Add(profile);
    }

    public async Task<Profile[]> GetProfiles(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));

        var userProfiles = await _repository.Find(x => x.UserId == userId);
        return userProfiles.ToArray();
    }
}
