namespace Bhasha.Common.Exceptions
{
    public class NotFoundException : HttpResponseException
    {
        public NotFoundException(string message) : base(404, message)
        {

        }
    }
}
