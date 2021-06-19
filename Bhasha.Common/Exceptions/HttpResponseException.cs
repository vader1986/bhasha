using System;

namespace Bhasha.Common.Exceptions
{
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; }

        public HttpResponseException(int statusCode, string message = default!, Exception innerException = default!) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
