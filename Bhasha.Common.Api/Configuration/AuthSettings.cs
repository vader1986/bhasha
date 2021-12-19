using Microsoft.Extensions.Configuration;

namespace Bhasha.Common.Api.Configuration
{
    public class AuthSettings
    {
        public string Scope { get; }
        public string AuthServer { get; }

        public AuthSettings(string scope, string authServer)
        {
            Scope = scope;
            AuthServer = authServer;
        }

        public static AuthSettings From(IConfiguration configuration)
        {
            return new AuthSettings(
                configuration["Authorization:Scope"],
                configuration["Authorization:AuthServer"]);
        }
    }
}
