using System;
namespace Bhasha.Common.MongoDB.Exceptions
{
    public class InvalidUserProgressException : Exception
    {
        public InvalidUserProgressException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
