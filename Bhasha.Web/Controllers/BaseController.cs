using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        //private readonly UserManager<IdentityUser> _userManager;

        protected BaseController(/*UserManager<IdentityUser> userManager*/)
        {
            //_userManager = userManager;
        }

        public string UserId => "test"; // _userManager.GetUserId(User); 
    }
}
