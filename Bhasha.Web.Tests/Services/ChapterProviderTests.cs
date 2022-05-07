using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services
{
	public class ChapterProviderTests
	{
		private readonly IRepository<Chapter> _chapterRepository;
		private readonly IRepository<Profile> _profileRepository;
		private readonly ITranslationProvider _translationProvider;
		private readonly IAsyncFactory<Page, Profile, DisplayedPage> _pageFactory;
		private readonly ChapterProvider _chapterProvider;

		public ChapterProviderTests()
        {
			_chapterRepository = Substitute.For<IRepository<Chapter>>();
			_profileRepository = Substitute.For<IRepository<Profile>>();
			_translationProvider = Substitute.For<ITranslationProvider>();
			_pageFactory = Substitute.For<IAsyncFactory<Page, Profile, DisplayedPage>>();
			_chapterProvider = new ChapterProvider(_chapterRepository, _profileRepository, _translationProvider, _pageFactory);
		}

		[Fact]
		public void GivenNoProfile_WhenGetChapters_ThenThrowException()
		{
			// setup
			_profileRepository.Get(default).ReturnsForAnyArgs(default(Profile));

			// act & verify
			Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
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

		[Theory, AutoData]
		public void GivenNoProfile_WhenGetChapter_ThenThrowException(Chapter chapter)
        {
			// setup
			_chapterRepository.Get(chapter.Id).Returns(chapter);

			// act & verify
			Assert.ThrowsAsync<ArgumentOutOfRangeException>(
				async () => await _chapterProvider.GetChapter(Guid.Empty, chapter.Id));
		}

		[Theory, AutoData]
		public void GivenNoChapter_WhenGetChapter_ThenThrowException(Profile profile)
		{
			// setup
			_profileRepository.Get(profile.Id).Returns(profile);

			// act & verify
			Assert.ThrowsAsync<ArgumentOutOfRangeException>(
				async () => await _chapterProvider.GetChapter(profile.Id, Guid.Empty));
		}

		[Theory, AutoData]
		public void GivenMissingNameTranslation_WhenGetChapter_ThenThrowException(
			Profile profile, Chapter chapter, DisplayedPage page, Translation description)
        {
			// setup
			_profileRepository.Get(profile.Id).Returns(profile);
			_chapterRepository.Get(chapter.Id).Returns(chapter);
			_pageFactory.CreateAsync(Arg.Any<Page>(), profile).Returns(page);
			_translationProvider.Find(chapter.DescriptionId, profile.Native).Returns(description);

			// act & verify
			Assert.ThrowsAsync<InvalidOperationException>(
				async () => await _chapterProvider.GetChapter(profile.Id, chapter.Id));
		}

		[Theory, AutoData]
		public void GivenMissingDescriptionTranslation_WhenGetChapter_ThenThrowException(
			Profile profile, Chapter chapter, DisplayedPage page, Translation name)
		{
			// setup
			_profileRepository.Get(profile.Id).Returns(profile);
			_chapterRepository.Get(chapter.Id).Returns(chapter);
			_pageFactory.CreateAsync(Arg.Any<Page>(), profile).Returns(page);
			_translationProvider.Find(chapter.NameId, profile.Native).Returns(name);

			// act & verify
			Assert.ThrowsAsync<InvalidOperationException>(
				async () => await _chapterProvider.GetChapter(profile.Id, chapter.Id));
		}

		[Theory, AutoData]
		public async Task GivenAllData_WhenGetChapter_ThenReturnDisplayedChapter(
			Profile profile, Chapter chapter, DisplayedPage page, Translation name, Translation description)
		{
			// setup
			_profileRepository.Get(profile.Id).Returns(profile);
			_chapterRepository.Get(chapter.Id).Returns(chapter);
			_pageFactory.CreateAsync(Arg.Any<Page>(), profile).Returns(page);
			_translationProvider.Find(chapter.NameId, profile.Native).Returns(name);
			_translationProvider.Find(chapter.DescriptionId, profile.Native).Returns(description);

			// act
			var displayedChapter = await _chapterProvider.GetChapter(profile.Id, chapter.Id);

			// verify
			var expectedName = name.Native;
			var expectedDescription = description.Native;
			var expectedPages = Enumerable.Repeat(page, chapter.Pages.Length).ToArray();

			displayedChapter.Should().Match<DisplayedChapter>(x =>
				x.Id == chapter.Id &&
				x.Name == expectedName &&
				x.Description == expectedDescription &&
				x.Pages.SequenceEqual(expectedPages) &&
				x.ResourceId == chapter.ResourceId);
		}
	}
}

