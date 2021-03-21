using System;

namespace Bhasha.Common.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(Type type, Guid id) : base($"{type.Name} with ID {id} not found")
        {

        }
    }
}
