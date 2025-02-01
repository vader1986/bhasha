namespace Bhasha.Identity;

public class DefaultUser
{
	public const string SectionName = "DefaultUsers";
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string Role { get; set; } = string.Empty;
}

public class IdentitySettings(DefaultUser[] defaultUsers)
{
	public const string SectionName = "Identity";
	public DefaultUser[] DefaultUsers { get; } = defaultUsers;

	public static IdentitySettings From(IConfiguration configuration)
	{
		var section = configuration.GetSection($"{SectionName}:{DefaultUser.SectionName}");
		var defaultUsers = section.Get<DefaultUser[]>() ?? [];

		return new IdentitySettings(defaultUsers);
	}
}