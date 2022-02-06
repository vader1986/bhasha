using AutoFixture.NUnit3;
using Bhasha.Web.Domain;
using Bhasha.Web.Services;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services
{
	public class TranslationExtensionsTests
	{
		[Test, AutoData]
		public void GivenTranslation_WhenMerged_ThenUpdateAudioId(Translation lhs, Translation rhs)
        {
			lhs = lhs with { AudioId = "old.wav" };
			rhs = rhs with { AudioId = "new.wav" };

			var combined = lhs.Merge(rhs);

			Assert.That(combined.AudioId, Is.EqualTo("new.wav"));
        }

		[Test, AutoData]
		public void GivenTranslation_WhenMerged_ThenUpdateSpoken(Translation lhs, Translation rhs)
		{
			lhs = lhs with { Spoken = "old" };
			rhs = rhs with { Spoken = "new" };

			var combined = lhs.Merge(rhs);

			Assert.That(combined.Spoken, Is.EqualTo("new"));
		}

		[Test, AutoData]
		public void GivenTranslation_WhenMerged_ThenLanguageAndNativeRemains(Translation lhs, Translation rhs)
		{
			var combined = lhs.Merge(rhs);

			Assert.That(combined.Language, Is.EqualTo(lhs.Language));
			Assert.That(combined.Native, Is.EqualTo(lhs.Native));
		}
	}
}

