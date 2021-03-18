using System;

namespace Bhasha.Common
{
    public class User : IEntity
    {
        /// <summary>
        /// Unqiue identifier for the user.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Username choosen by the user.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// E-Mail address associated with this user.
        /// </summary>
        public string Email { get; }

        public User(Guid id, string userName, string email)
        {
            Id = id;
            UserName = userName;
            Email = email;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(UserName)}: {UserName}, {nameof(Email)}: {Email}";
        }
    }
}
