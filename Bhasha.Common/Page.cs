﻿namespace Bhasha.Common
{
    public class Page
    {
        /// <summary>
        /// Type of page used to learn a new part of speech. 
        /// </summary>
        public PageType PageType { get; }

        /// <summary>
        /// Language-idependent description of the new part of speech.
        /// </summary>
        public Token Token { get; }

        /// <summary>
        /// The part of speech in the original language.
        /// </summary>
        public Translation Translation { get; }

        /// <summary>
        /// Arguments for the page used to help the user learn the part of speech.
        /// </summary>
        public object Arguments { get; }

        public Page(PageType pageType, Token token, Translation translation, object arguments)
        {
            PageType = pageType;
            Token = token;
            Translation = translation;
            Arguments = arguments;
        }
    }
}
