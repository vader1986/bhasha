using System;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Exceptions
{
    public class InvalidObjectException : Exception
    {
        public InvalidObjectException(object @object) : base($"Invalid {@object.GetType().Name}: {@object.Stringify()}")
        {
        }
    }
}
