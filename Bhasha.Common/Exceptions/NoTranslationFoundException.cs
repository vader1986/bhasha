using System;
namespace Bhasha.Common.Exceptions
{
    public class NoTranslationFoundException : Exception
    {
        public NoTranslationFoundException(string message, Exception? innerException = default) : base(message, innerException)
        {
        }
    }
}
