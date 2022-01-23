namespace Bhasha.Web.Identity
{
	public class DefaultUser
    {
		public string Email { get; set; }
		public string Password { get; set; }
		public string Role { get; set; }
	}

	public class IdentitySettings
	{
		public DefaultUser[] DefaultUsers { get; set; }

		public static IdentitySettings From(IConfiguration configuration)
		{
			var section = configuration.GetSection("Identity:DefaultUsers");
			var defaultUsers = section.Get<DefaultUser[]>();

			return new IdentitySettings
			{
				DefaultUsers = defaultUsers
			};
		}
	}
}