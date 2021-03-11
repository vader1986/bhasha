namespace Bhasha.Common
{
    public class Page
    {
        /// <summary>
        /// Metadata of the word to learn inside of this page. 
        /// </summary>
        public Token Token { get; }

        /// <summary>
        /// Type of page used to learn a new part of speech. 
        /// </summary>
        public PageType PageType { get; }

        /// <summary>
        /// The part of speech in the original language.
        /// </summary>
        public LanguageToken Word { get; }

        /// <summary>
        /// Arguments for the page used to help the user learn the part of speech.
        /// Those arguments could contain parts of the solution or multiple solutions
        /// the user has to choose from. 
        /// </summary>
        public string[] Arguments { get; }

        public Page(Token token, PageType pageType, LanguageToken word, string[] arguments)
        {
            Token = token;
            PageType = pageType;
            Word = word;
            Arguments = arguments;
        }
    }
}
