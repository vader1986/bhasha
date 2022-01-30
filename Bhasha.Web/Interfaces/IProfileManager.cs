using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces;

public interface IProfileManager
{
	Task<Profile> Create(string userId, Language native, Language target);
}

