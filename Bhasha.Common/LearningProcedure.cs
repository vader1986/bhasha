using System.Collections.Generic;

namespace Bhasha.Common
{
    public class LearningProcedure
    {
        public IEnumerable<Translation> Pool { get; }
        public Procedure Procedure { get; }

        public LearningProcedure(IEnumerable<Translation> pool, Procedure procedure)
        {
            Pool = pool;
            Procedure = procedure;
        }
    }
}
