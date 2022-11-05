using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Identity;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BlockAttribute : TypeFilterAttribute
{
    public BlockAttribute() : base(typeof(BlockFilter))
    {
    }
}