using System;

namespace Bhasha.Web.Api.Exceptions
{
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; }

        public HttpResponseException(int statusCode) : this(statusCode, default)
        {

        }

        public HttpResponseException(int statusCode, string message) : this(statusCode, message, default)
        {

        }

        public HttpResponseException(int statusCode, string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
