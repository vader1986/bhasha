using System;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Translation of a single <see cref="Word"/>. 
    /// </summary>
    public class DbTranslation : ICanBeValidated
    {
        /// <summary>
        /// Word translated into <see cref="Language"/> written in its native script.
        /// </summary>
        public string? Native { get; set; }

        /// <summary>
        /// Word translated into <see cref="Language"/> written in spoken language. 
        /// </summary>
        public string? Spoken { get; set; }

        /// <summary>
        /// Identifier of the audio file for the word.
        /// </summary>
        public ResourceId? AudioId { get; set; }
        
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Native) || string.IsNullOrWhiteSpace(Spoken))
            {
                throw new InvalidObjectException(this);
            }
        }
    }
}
