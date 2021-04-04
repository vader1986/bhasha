using System;

namespace Bhasha.Common
{
    public class Evaluation : IEquatable<Evaluation>
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

        public bool Equals(Evaluation other)
        {
            return other != null && other.Result == Result && other.Submit.Equals(Submit) && other.Profile.Equals(Profile);
        }
    }
}
