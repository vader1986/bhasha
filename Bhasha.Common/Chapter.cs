namespace Bhasha.Common
{
    public class Chapter
    {
        public Category Category { get; }
        public Translation[] Translations { get; }
        public Procedure[] Procedures { get; }

        public Chapter(Category category, Translation[] translations, Procedure[] procedures)
        {
            Category = category;
            Translations = translations;
            Procedures = procedures;
        }
    }
}
