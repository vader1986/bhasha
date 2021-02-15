#nullable enable
using System.Collections.Generic;

namespace Bhasha.Common
{
    public class LearningProcedure
    {
        public IEnumerable<Translation> Pool { get; }
        public IEnumerable<ResourceId> Tutorial { get; }

        public LearningProcedure(IEnumerable<Translation> pool, IEnumerable<ResourceId> tutorial)
        {
            Pool = pool;
            Tutorial = tutorial;
        }
    }
}
