using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    public class DbTranslatedPage : ICanBeValidated
    {
        /// <summary>
        /// Type of page used to learn a new part of speech. 
        /// </summary>
        public PageType PageType { get; set; }

        /// <summary>
        /// Expression translated into native language.
        /// </summary>
        public DbTranslatedExpression? Native { get; set; }

        /// <summary>
        /// Expression translated into target language.
        /// </summary>
        public DbTranslatedExpression? Target { get; set; }

        public void Validate()
        {
            if (Native == null || Target == null)
            {
                throw new InvalidObjectException(this);
            }

            Native.Validate();
            Target.Validate();
        }
    }
}
