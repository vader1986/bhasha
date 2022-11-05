using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces;

public interface IProfileManager
{
	Task<Profile> Create(string userId, LangKey languages);
	Task<Profile[]> GetProfiles(string userId);
}

