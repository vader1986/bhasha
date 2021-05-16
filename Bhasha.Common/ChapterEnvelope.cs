using System;

namespace Bhasha.Common
{
    public class ChapterEnvelope : IEquatable<ChapterEnvelope?>
    {
        /// <summary>
        /// Actual chapter associated with the <see cref="Stats"/>.
        /// </summary>
        public Chapter Chapter { get; }

        /// <summary>
        /// <see cref="Stats"/> of the user for the <see cref="Chapter"/>.
        /// </summary>
        public Stats Stats { get; }

        public ChapterEnvelope(Chapter chapter, Stats stats)
        {
            Chapter = chapter;
            Stats = stats;
        }

        public override string ToString()
        {
            return $"{nameof(Chapter)}: {Chapter}, {nameof(Stats)}: {Stats}";
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ChapterEnvelope);
        }

        public bool Equals(ChapterEnvelope? other)
        {
            return other != null && Chapter == other.Chapter && Stats == other.Stats;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Chapter, Stats);
        }

        public static bool operator ==(ChapterEnvelope? left, ChapterEnvelope? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ChapterEnvelope? left, ChapterEnvelope? right)
        {
            return !(left == right);
        }
    }
}
