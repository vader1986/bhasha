namespace Bhasha.Common
{
    public class LearningProcedure
    {
        public Translation[] Pool { get; }
        public Procedure Procedure { get; }

        public LearningProcedure(Translation[] pool, Procedure procedure)
        {
            Pool = pool;
            Procedure = procedure;
        }
    }
}
