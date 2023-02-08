using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Grains;
using Bhasha.Services;
using FluentAssertions;
using NSubstitute;
using Orleans.TestKit;
using Xunit;

namespace Bhasha.Tests.Grains;

public class DisplayChapterGrainTests : TestKitBase
{
    private readonly IChapterRepository _chapterRepository = Substitute.For<IChapterRepository>();
    private readonly ITranslationRepository _translationProvider = Substitute.For<ITranslationRepository>();
    private readonly IPageFactory _pageFactory = Substitute.For<IPageFactory>();

    public DisplayChapterGrainTests()
	{
        Silo.AddService(_chapterRepository);
        Silo.AddService(_translationProvider);
        Silo.AddService(_pageFactory);
    }

    [Theory, AutoData]
    public async Task GivenDisplayChapter_WhenAllDataAvailable_ThenReturnDisplayedChapter(ChapterKey key, Chapter chapter, Translation translation, DisplayedPage page)
    {
        // setup
        key = key with { LangId = new LangKey(Language.English, Language.Bengali) };

        _chapterRepository
            .FindById(key.ChapterId)
            .Returns(chapter);

        _translationProvider
            .Find(Arg.Any<Guid>(), Arg.Any<Language>())
            .Returns(translation);

        _pageFactory
            .CreateAsync(Arg.Any<Page>(), key.LangId)
            .Returns(page);

        // act
        var grain = await Silo.CreateGrainAsync<DisplayChapterGrain>(key);
        var result = await grain.Display();

        // verify
        result.Name.Should().Be(translation.Text);
        result.Pages.Should().BeEquivalentTo(Enumerable.Range(0, chapter.Pages.Length).Select(_ => page));
    }

    [Theory, AutoData]
    public async Task GivenDisplayChapter_WhenChapterDoesNotExist_ThenThrowException(ChapterKey key)
    {
        // setup
        key = key with { LangId = new LangKey(Language.English, Language.Bengali) };

        // act
        var grain = await Silo.CreateGrainAsync<DisplayChapterGrain>(key);
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async() => await grain.Display());

        // verify
        exception.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task GivenDisplayChapter_WhenNameTranslationDoesNotExist_ThenThrowException(ChapterKey key, Chapter chapter, Translation translation, DisplayedPage page)
    {
        // setup
        key = key with { LangId = new LangKey(Language.English, Language.Bengali) };

        _chapterRepository
            .FindById(key.ChapterId)
            .Returns(chapter);

        _translationProvider
            .Find(chapter.DescriptionId, Arg.Any<Language>())
            .Returns(translation);

        _pageFactory
            .CreateAsync(Arg.Any<Page>(), key.LangId)
            .Returns(page);

        // act
        var grain = await Silo.CreateGrainAsync<DisplayChapterGrain>(key);
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await grain.Display());

        // verify
        exception.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task GivenDisplayChapter_WhenDescriptionTranslationDoesNotExist_ThenThrowException(ChapterKey key, Chapter chapter, Translation translation, DisplayedPage page)
    {
        // setup
        key = key with { LangId = new LangKey(Language.English, Language.Bengali) };

        _chapterRepository
            .FindById(key.ChapterId)
            .Returns(chapter);

        _translationProvider
            .Find(chapter.NameId, Arg.Any<Language>())
            .Returns(translation);

        _pageFactory
            .CreateAsync(Arg.Any<Page>(), key.LangId)
            .Returns(page);

        // act
        var grain = await Silo.CreateGrainAsync<DisplayChapterGrain>(key);
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await grain.Display());

        // verify
        exception.Should().NotBeNull();
    }
}

