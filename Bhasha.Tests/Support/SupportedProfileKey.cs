using System.Linq;
using Bhasha.Shared.Domain;

namespace Bhasha.Tests.Support;

public static class SupportedProfileKey
{
    public static ProfileKey Create()
    {
        var native = Language.Supported.Keys.Random();
        var target = Language.Supported.Keys.Where(key => key != native).Random();

        return new ProfileKey("user-123", native, target);
    }
}

