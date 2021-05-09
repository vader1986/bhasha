using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Model class for language profiles.
    /// </summary>
    public class DbProfile : ICanBeValidated
    {
        /// <summary>
        /// Native language of the user profile.
        /// </summary>
        public string? Native { get; set; }

        /// <summary>
        /// Target language to learn.
        /// </summary>
        public string? Target { get; set; }

        public void Validate()
        {
            if (Native == null || Target == null || Native == Language.Unknown || Target == Language.Unknown)
            {
                throw new InvalidObjectException(this);
            }
        }
    }
}
