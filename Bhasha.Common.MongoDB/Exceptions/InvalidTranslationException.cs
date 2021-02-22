using System;
namespace Bhasha.Common.MongoDB.Exceptions
{
    public class InvalidTranslationException : Exception
    {
        public InvalidTranslationException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
