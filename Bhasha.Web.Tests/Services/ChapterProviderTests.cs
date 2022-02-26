using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services
{
	public class ChapterProviderTests
	{
		private readonly IRepository<Chapter> _chapterRepository;
		private readonly IRepository<Profile> _profileRepository;
		private readonly ITranslationProvider _translationProvider;
		private readonly ChapterProvider _chapterProvider;

		public ChapterProviderTests()
        {
			_chapterRepository = Substitute.For<IRepository<Chapter>>();
			_profileRepository = Substitute.For<IRepository<Profile>>();
			_translationProvider = Substitute.For<ITranslationProvider>();
			_chapterProvider = new ChapterProvider(_chapterRepository, _profileRepository, _translationProvider);
		}

		[Fact]
		public void GivenNoProfile_WhenGetChapters_ThenThrowException()
		{
			// setup
			_profileRepository.Get(default).ReturnsForAnyArgs(default(Profile));

			// act & verify
			Assert.ThrowsAsync<InvalidOperationException>(async () =>
				await _chapterProvider.GetChapters(Guid.Empty));
		}

		[Theory, AutoData]
		public async Task GivenProfileAndChapter_WhenGetChapters_ThenReturnChapterDescriptions(Profile profile, Chapter[] chapters, Translation translation)
        {
			// setup
			profile = profile with { Native = Language.Bengali };
			var translations = chapters
				.Select(x => x.NameId).Concat(chapters.Select(x => x.DescriptionId))
				.ToDictionary(x => x, _ => translation);

			_profileRepository.Get(profile.Id).Returns(profile);
			_chapterRepository.Find(default!).ReturnsForAnyArgs(chapters);
			_translationProvider.FindAll(default!, default!).ReturnsForAnyArgs(translations);

			// act
			var result = await _chapterProvider.GetChapters(profile.Id);

			// verify
			Assert.Equal(chapters.Length, result.Length);
		}
	}
}

