using System;
namespace Bhasha.Common.MongoDB.Exceptions
{
    public class InvalidProcedureException : Exception
    {
        public InvalidProcedureException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
