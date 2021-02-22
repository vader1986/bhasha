using System;
namespace Bhasha.Common.Exceptions
{
    public class NoProcedureFoundException : Exception
    {
        public NoProcedureFoundException(string message, Exception? innerException = default) : base(message, innerException)
        {

        }
    }
}
