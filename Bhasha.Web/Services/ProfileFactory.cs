using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services;

public class ProfileFactory : IFactory<Profile>
{
    public Profile Create()
    {
        return new Profile(Guid.Empty, String.Empty, Language.English, Language.Bengali, 0, 0);
    }
}

