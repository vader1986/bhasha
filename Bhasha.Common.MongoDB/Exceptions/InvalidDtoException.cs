using System;
using System.Linq;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.MongoDB.Exceptions
{
    public class InvalidDtoException : Exception
    {
        private static string CreateMessage(object[] dtos)
        {
            var objects = string.Join(", ",
                dtos.Select(x => $"{x.GetType().Name} {x?.Stringify()}"));

            return $"failed to convert {objects}";
        }

        public InvalidDtoException(params object[] dtos) : base(CreateMessage(dtos))
        {
        }

        public InvalidDtoException(Exception innerException, params object[] dtos) : base(CreateMessage(dtos), innerException)
        {
        }
    }
}
