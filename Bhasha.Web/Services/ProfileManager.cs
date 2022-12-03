using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services;

public class ProfileManager : IProfileManager
{
    private readonly IRepository<Profile> _repository;
    
    public ProfileManager(IRepository<Profile> repository)
    {
        _repository = repository;
    }

    public async Task<Profile> Create(string userId, LangKey languages)
    {
        var native = (Language)languages.Native;
        var target = (Language)languages.Target;

        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));

        if (!native.IsSupported())
            throw new ArgumentException($"Native language {native} is not supported", nameof(native));

        if (!target.IsSupported())
            throw new ArgumentException($"Native language {target} is not supported", nameof(target));

        if (native == target)
            throw new ArgumentException("Native and target language cannot be equal", nameof(native));

        var existingProfile = await _repository.Find(
            x => x.Key.UserId == userId && x.Key.LangId.Native == native && x.Key.LangId.Target == target);

        if (existingProfile.Any())
            throw new InvalidOperationException($"Profile {native} - {target} for user {userId} already exists");

        var key = new ProfileKey(userId, new LangKey(native, target));

        return await _repository.Add(Profile.Empty(key));
    }

    public async Task<Profile[]> GetProfiles(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));

        var userProfiles = await _repository.Find(x => x.Key.UserId == userId);
        return userProfiles.ToArray();
    }
}
