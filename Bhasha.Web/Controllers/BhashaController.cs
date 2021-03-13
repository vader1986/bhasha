using System;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    public abstract class BhashaController : Controller
    {
        // TODO
        // Use Identity framework to get the user ID from this.User or
        // whatever we can use ...
        public Guid UserId => default; 
    }
}
