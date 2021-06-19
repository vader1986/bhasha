using System.Security.Claims;
using Bhasha.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Student.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        public string UserId
        {
            get
            {
                return User?.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? "test";//throw new UnauthorizedException();
            }
        }
    }
}
