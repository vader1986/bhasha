using System;

namespace Bhasha.Common
{
    public class Evaluation : IEquatable<Evaluation?>
    {
        /// <summary>
        /// Evaluation <see cref="Common.Result"/> for a <see cref="Submit"/>.
        /// </summary>
        public Result Result { get; }

        /// <summary>
        /// Original submit of the evaluation.
        /// </summary>
        public Submit Submit { get; }

        /// <summary>
        /// Updated profile.
        /// </summary>
        public Profile Profile { get; }

        public Evaluation(Result result, Submit submit, Profile profile)
        {
            Result = result;
            Submit = submit;
            Profile = profile;
        }

        public override string ToString()
        {
            return $"{nameof(Result)}: {Result}, {nameof(Submit)}: {Submit}, {nameof(Profile)}: {Profile}";
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Evaluation);
        }

        public bool Equals(Evaluation? other)
        {
            return other != null && Result == other.Result && Submit == other.Submit && Profile == other.Profile;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Result, Submit, Profile);
        }

        public static bool operator ==(Evaluation? left, Evaluation? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Evaluation? left, Evaluation? right)
        {
            return !(left == right);
        }
    }
}
