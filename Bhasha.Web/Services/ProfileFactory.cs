using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services;

public class ProfileFactory : IFactory<Profile>
{
    public Profile Create()
    {
        return new Profile(
            Guid.Empty,
            string.Empty,
            LangKey.Unknown,
            new Progress(1, Guid.Empty, Array.Empty<Guid>(), 0, Array.Empty<ValidationResultType>()));
    }
}

