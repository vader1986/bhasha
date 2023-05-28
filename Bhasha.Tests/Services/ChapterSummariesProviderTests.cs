using AutoFixture.Xunit2;
using Bhasha.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Domain.Interfaces;
using Bhasha.Grains;
using Bhasha.Services;
using NSubstitute;
using Orleans.Runtime;
using Xunit;
using FluentAssertions;

namespace Bhasha.Tests.Services;

public class ChapterSummariesProviderTests
{
    private readonly IChapterRepository _chapterRepository = Substitute.For<IChapterRepository>();
    private readonly ITranslationRepository _translationProvider = Substitute.For<ITranslationRepository>();
    private readonly ChapterSummariesProvider _provider;

    public ChapterSummariesProviderTests()
	{
        _provider = new ChapterSummariesProvider(_chapterRepository, _translationProvider);
    }

    [Theory, AutoData]
    public async Task GivenGetSummariesCall_WhenAllDataAvailable_ThenReturnExpectedSummaries(int level, Chapter[] chapters, Translation translation)
    {
        // setup
        var languages = new LangKey(Language.English, Language.Bengali);

        _chapterRepository
            .FindByLevel(level)
            .Returns(chapters.ToAsyncEnumerable());

        _translationProvider
            .Find(Arg.Any<Guid>(), Language.English)
            .Returns(translation);

        // act
        var result = await _provider.GetSummaries(level, languages);

        // verify
        var expectedSummaries = chapters.Select(x => new Summary(x.Id, translation.Text, translation.Text));
        result.Should().BeEquivalentTo(expectedSummaries);
    }

    [Theory, AutoData]
    public async Task GivenGetSummariesCall_WhenMissingTranslation_ThenThrowException(int level, Chapter[] chapters)
    {
        // setup
        var languages = new LangKey(Language.English, Language.Bengali);

        _chapterRepository
            .FindByLevel(level)
            .Returns(chapters.ToAsyncEnumerable());

        // act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _provider.GetSummaries(level, languages));

        // verify
        exception.Should().NotBeNull();
    }
}

