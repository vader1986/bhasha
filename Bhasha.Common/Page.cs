using System;

namespace Bhasha.Common
{
    public class Page : IEquatable<Page?>
    {
        /// <summary>
        /// Type of page used to learn a new part of speech. 
        /// </summary>
        public PageType PageType { get; }

        /// <summary>
        /// Expression translated in the original language.
        /// </summary>
        public TranslatedExpression Translation { get; }

        /// <summary>
        /// Arguments for the page used to help the user learn the part of speech.
        /// </summary>
        public object Arguments { get; }

        public Page(PageType pageType, TranslatedExpression translation, object arguments)
        {
            PageType = pageType;
            Translation = translation;
            Arguments = arguments;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Page);
        }

        public bool Equals(Page? other)
        {
            return other != null && PageType == other.PageType && Translation == other.Translation && Arguments == other.Arguments;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PageType, Translation, Arguments);
        }

        public static bool operator ==(Page? left, Page? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Page? left, Page? right)
        {
            return !(left == right);
        }
    }
}
