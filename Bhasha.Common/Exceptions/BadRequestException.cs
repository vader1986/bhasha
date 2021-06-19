namespace Bhasha.Common.Exceptions
{
    public class BadRequestException : HttpResponseException
    {
        public BadRequestException(string message) : base(400, message)
        {

        }
    }
}
