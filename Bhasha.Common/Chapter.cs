namespace Bhasha.Common
{
    public class Chapter
    {
        public Translation[] Translations { get; }
        public Procedure[] Procedures { get; }

        public Chapter(Translation[] translations, Procedure[] procedures)
        {
            Translations = translations;
            Procedures = procedures;
        }
    }
}
