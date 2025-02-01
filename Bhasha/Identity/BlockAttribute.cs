using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Identity;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BlockAttribute() : TypeFilterAttribute(typeof(BlockFilter));