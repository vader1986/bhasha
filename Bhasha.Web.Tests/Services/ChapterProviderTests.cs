using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services
{
	public class ChapterProviderTests
	{
		private IRepository<Chapter> _chapterRepository = default!;
		private IRepository<Profile> _profileRepository = default!;
		private ITranslationProvider _translationProvider = default!;
		private ChapterProvider _chapterProvider = default!;

		[SetUp]
		public void Before()
        {
			_chapterRepository = A.Fake<IRepository<Chapter>>();
			_profileRepository = A.Fake<IRepository<Profile>>();
			_translationProvider = A.Fake<ITranslationProvider>();
			_chapterProvider = new ChapterProvider(_chapterRepository, _profileRepository, _translationProvider);
		}

		[Test]
		public void GivenNoProfile_WhenGetChapters_ThenThrowException()
		{
			// setup
			A.CallTo(() => _profileRepository.Get(A<Guid>.Ignored)).Returns(default(Profile));

			// act & verify
			Assert.ThrowsAsync<InvalidOperationException>(async () =>
				await _chapterProvider.GetChapters(Guid.Empty));
		}

		[Test, AutoData]
		public async Task GivenProfileAndChapter_WhenGetChapters_ThenReturnChapterDescriptions(Profile profile, Chapter[] chapters, Translation translation)
        {
			// setup
			profile = profile with { Native = Language.Bengali };
			var translations = chapters
				.Select(x => x.NameId).Concat(chapters.Select(x => x.DescriptionId))
				.ToDictionary(x => x, _ => translation);

			A.CallTo(() => _profileRepository.Get(profile.Id)).Returns(profile);

			A.CallTo(() => _chapterRepository.Find(A<Expression<Func<Chapter, bool>>>.Ignored))
				.Returns(Task.FromResult(chapters));

			A.CallTo(() => _translationProvider.FindAll(profile.Native, A<Guid[]>.Ignored))
				.Returns(Task.FromResult<IDictionary<Guid, Translation>>(translations));

			// act
			var result = await _chapterProvider.GetChapters(profile.Id);

			// verify
			Assert.That(result.Length, Is.EqualTo(chapters.Length));
		}
	}
}

