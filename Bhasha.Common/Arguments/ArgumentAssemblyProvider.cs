using System;
using System.Collections.Generic;

namespace Bhasha.Common.Arguments
{
    public class ArgumentAssemblyProvider : IArgumentAssemblyProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ArgumentAssemblyProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAssembleArguments GetAssembly(PageType key)
        {
            return key switch
            {
                PageType.OneOutOfFour => (IAssembleArguments)_serviceProvider
                    .GetService(typeof(OneOutOfFourArgumentsAssembly)),

                _ => throw new KeyNotFoundException(
                    $"No {nameof(IAssembleArguments)} found for {key}"),
            };
        }
    }
}
