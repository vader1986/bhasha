namespace Bhasha.Common
{
    public class User
    {
        /// <summary>
        /// Username choosen by the user.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// E-Mail address associated with this user.
        /// </summary>
        public string Email { get; }

        public User(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }

        public override string ToString()
        {
            return $"{nameof(UserName)}: {UserName}, {nameof(Email)}: {Email}";
        }
    }
}
