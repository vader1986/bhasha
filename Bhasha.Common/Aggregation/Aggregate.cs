namespace Bhasha.Common.Aggregation
{
    public static class Aggregate
    {
        public static ILoadChapter CreateLoader(IDatabase database)
        {
            return new ChapterLoader(
                new TranslationLoader(database),
                new ProcedureLoader(database));
        }
    }
}
