using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Grains;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Tests.Support;
using FluentAssertions;
using NSubstitute;
using Orleans.TestKit;
using Xunit;

namespace Bhasha.Web.Tests.Grains;

public class DisplayChapterGrainTests : TestKitBase
{
    private readonly IRepository<Chapter> _chapterRepository;
    private readonly ITranslationProvider _translationProvider;
    private readonly IAsyncFactory<Page, LangKey, DisplayedPage> _pageFactory;

    public DisplayChapterGrainTests()
	{
        _chapterRepository = Substitute.For<IRepository<Chapter>>();
        _translationProvider = Substitute.For<ITranslationProvider>();
        _pageFactory = Substitute.For<IAsyncFactory<Page, LangKey, DisplayedPage>>();

        Silo.AddService(_chapterRepository);
        Silo.AddService(_translationProvider);
        Silo.AddService(_pageFactory);
    }

    [Theory, AutoData]
    public async Task GivenChapterTranslationsAndPages_WhenDisplay_ThenReturnDisplayedChapter(
        Chapter chapter, Translation name, Translation description, DisplayedPage page)
    {
        // setup
        var chapterKey = new ChapterKey(chapter.Id, SupportedLanguageKey.Create());

        _chapterRepository
            .Get(chapterKey.ChapterId)
            .Returns(chapter);

        _pageFactory
            .CreateAsync(default!, default!)
            .ReturnsForAnyArgs(page);

        _translationProvider
            .Find(chapter.NameId, chapterKey.LangId.Native)
            .Returns(name);

        _translationProvider
            .Find(chapter.DescriptionId, chapterKey.LangId.Native)
            .Returns(description);

        // act
        var grain = await Silo.CreateGrainAsync<DisplayChapterGrain>(chapterKey.ToString());
        var result = await grain.Display();

        // verify
        result.Id.Should().Be(chapter.Id);
        result.Name.Should().Be(name.Native);
        result.Description.Should().Be(description.Native);
        result.ResourceId.Should().Be(chapter.ResourceId);

        var expectedPages = Enumerable
            .Range(0, chapter.Pages.Length)
            .Select(_ => page);

        result.Pages.Should().BeEquivalentTo(expectedPages);
    }

    [Theory, AutoData]
    public async Task GivenMissingChapter_WhenDisplay_ThenThrowException(
        Chapter chapter, Translation name, Translation description, DisplayedPage page)
    {
        // setup
        var chapterKey = new ChapterKey(chapter.Id, SupportedLanguageKey.Create());

        _chapterRepository
            .Get(chapterKey.ChapterId)
            .Returns(default(Chapter?));

        _pageFactory
            .CreateAsync(default!, default!)
            .ReturnsForAnyArgs(page);

        _translationProvider
            .Find(chapter.NameId, chapterKey.LangId.Native)
            .Returns(name);

        _translationProvider
            .Find(chapter.DescriptionId, chapterKey.LangId.Native)
            .Returns(description);

        // act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async() =>
        {
            var grain = await Silo.CreateGrainAsync<DisplayChapterGrain>(chapterKey.ToString());
            await grain.Display();
        });

        // verify
        exception.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task GivenMissingNameTranslation_WhenDisplay_ThenThrowException(
    Chapter chapter, Translation description, DisplayedPage page)
    {
        // setup
        var chapterKey = new ChapterKey(chapter.Id, SupportedLanguageKey.Create());

        _chapterRepository
            .Get(chapterKey.ChapterId)
            .Returns(default(Chapter?));

        _pageFactory
            .CreateAsync(default!, default!)
            .ReturnsForAnyArgs(page);

        _translationProvider
            .Find(chapter.NameId, chapterKey.LangId.Native)
            .Returns(default(Translation?));

        _translationProvider
            .Find(chapter.DescriptionId, chapterKey.LangId.Native)
            .Returns(description);

        // act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            var grain = await Silo.CreateGrainAsync<DisplayChapterGrain>(chapterKey.ToString());
            await grain.Display();
        });

        // verify
        exception.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task GivenMissingDescriptionTranslation_WhenDisplay_ThenThrowException(
        Chapter chapter, Translation name, DisplayedPage page)
    {
        // setup
        var chapterKey = new ChapterKey(chapter.Id, SupportedLanguageKey.Create());

        _chapterRepository
            .Get(chapterKey.ChapterId)
            .Returns(default(Chapter?));

        _pageFactory
            .CreateAsync(default!, default!)
            .ReturnsForAnyArgs(page);

        _translationProvider
            .Find(chapter.NameId, chapterKey.LangId.Native)
            .Returns(name);

        _translationProvider
            .Find(chapter.DescriptionId, chapterKey.LangId.Native)
            .Returns(default(Translation?));

        // act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            var grain = await Silo.CreateGrainAsync<DisplayChapterGrain>(chapterKey.ToString());
            await grain.Display();
        });

        // verify
        exception.Should().NotBeNull();
    }
}