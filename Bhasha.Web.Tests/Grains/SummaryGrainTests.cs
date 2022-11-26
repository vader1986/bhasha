using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Grains;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Pages.Student;
using FluentAssertions;
using NSubstitute;
using Orleans.TestKit;
using Xunit;

namespace Bhasha.Web.Tests.Grains;

public class SummaryGrainTests : TestKitBase
{
    private readonly IChapterLookup _chapterLookup;
    private readonly ITranslationProvider _translationProvider;

    public SummaryGrainTests()
    {
        _chapterLookup = Substitute.For<IChapterLookup>();
        _translationProvider = Substitute.For<ITranslationProvider>();

        Silo.AddService(_chapterLookup);
        Silo.AddService(_translationProvider);
    }

    [Theory, AutoData]
    public async Task GivenChaptersAndTranslations_WhenGetSummaries_ThenReturnSummaries(Chapter[] chapters, Translation translation)
    {
        // setup
        var key = SummaryCollectionKey.Parse($"2-{Language.English}>{Language.Bengali}");
        var word = translation.Native;

        var expectedChapterIds = chapters
            .Select(chapter => chapter.Id);

        var expectedSummaries = expectedChapterIds
            .Select(chapterId => new Summary(chapterId, word, word));

        _chapterLookup
            .GetChapters(2)
            .Returns(chapters.ToAsyncEnumerable());

        _translationProvider
            .Find(Arg.Any<Guid>(), key.LangId.Native)
            .Returns(translation);

        // act
        var grain = await Silo.CreateGrainAsync<SummaryGrain>(key.ToString());
        var result = await grain.GetSummaries();

        // verify
        result
            .Should()
            .BeEquivalentTo(expectedSummaries);
    }

    [Theory, AutoData]
    public async Task GivenChaptersWithMatchingWords_WhenGetSummaries_ThenLoadTranslationForSameExpressionOnlyOnce(Chapter chapter, Translation translation)
    {
        // setup
        var key = SummaryCollectionKey.Parse($"2-{Language.English}>{Language.Bengali}");

        var expressionId = Guid.NewGuid();
        var chapters = new[]
        {
            chapter with { NameId = expressionId, DescriptionId = expressionId },
            chapter with { NameId = expressionId }
        };

        _chapterLookup
            .GetChapters(2)
            .Returns(chapters.ToAsyncEnumerable());

        _translationProvider
            .Find(Arg.Any<Guid>(), key.LangId.Native)
            .Returns(translation);

        // act
        var grain = await Silo.CreateGrainAsync<SummaryGrain>(key.ToString());
        await grain.GetSummaries();

        // verify
        await _translationProvider
            .Received(1)
            .Find(expressionId, key.LangId.Native);
    }

    [Theory, AutoData]
    public async Task GivenChapterWithMissingTranslation_WhenGetSummaries_ThenThrow(Chapter[] chapters)
    {
        // setup
        var key = SummaryCollectionKey.Parse($"2-{Language.English}>{Language.Bengali}");

        _chapterLookup
        .GetChapters(2)
            .Returns(chapters.ToAsyncEnumerable());

        _translationProvider
            .Find(Arg.Any<Guid>(), key.LangId.Native)
            .Returns(default(Translation?));

        // act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            var grain = await Silo.CreateGrainAsync<SummaryGrain>(key.ToString());
            await grain.GetSummaries();
        });

        // verify
        exception.Should().NotBeNull();
    }
}

