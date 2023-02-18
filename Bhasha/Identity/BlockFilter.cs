using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bhasha.Identity;

public class BlockFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        context.Result = new RedirectResult("/");
    }
}

