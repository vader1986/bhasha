namespace Bhasha.Web.Api.Exceptions
{
    public class UnauthorizedException : HttpResponseException
    {
        public UnauthorizedException(string message) : base(401, message)
        {

        }
    }
}
