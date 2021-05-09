using System;

namespace Bhasha.Common
{
    public class User : IEquatable<User?>
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

        public override bool Equals(object? obj)
        {
            return Equals(obj as User);
        }

        public bool Equals(User? other)
        {
            return other != null && UserName == other.UserName && Email == other.Email;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserName, Email);
        }

        public static bool operator ==(User? left, User? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(User? left, User? right)
        {
            return !(left == right);
        }
    }
}
