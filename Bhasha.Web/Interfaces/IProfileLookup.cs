using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces;

public interface IProfileLookup
{
    IAsyncEnumerable<Profile> GetProfiles(string userId);
}

