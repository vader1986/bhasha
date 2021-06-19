namespace Bhasha.Common.Exceptions
{
    public class UnauthorizedException : HttpResponseException
    {
        public UnauthorizedException() : base(401)
        {

        }

        public UnauthorizedException(string message) : base(401, message)
        {

        }
    }
}
