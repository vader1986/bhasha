using System;

namespace Bhasha.Common
{
    public class User
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
        public string EmailAddress { get; }

        public User(Guid id, string userName, string emailAddress)
        {
            Id = id;
            UserName = userName;
            EmailAddress = emailAddress;
        }
    }
}
