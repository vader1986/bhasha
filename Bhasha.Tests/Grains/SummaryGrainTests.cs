using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Grains;
using FluentAssertions;
using NSubstitute;
using Orleans.TestKit;
using Xunit;

namespace Bhasha.Tests.Grains;

public class SummaryGrainTests : TestKitBase
{
    private readonly IChapterRepository _chapterRepository = Substitute.For<IChapterRepository>();
    private readonly ITranslationRepository _translationProvider = Substitute.For<ITranslationRepository>();

    public SummaryGrainTests()
	{
        Silo.AddService(_chapterRepository);
        Silo.AddService(_translationProvider);
    }

    [Theory, AutoData]
    public async Task GivenGetSummariesCall_WhenAllDataAvailable_ThenReturnExpectedSummaries(SummaryCollectionKey key, Chapter[] chapters, Translation translation)
    {
        // setup
        key = key with { LangId = new LangKey(Language.English, Language.Bengali) };

        _chapterRepository
            .FindByLevel(key.Level)
            .Returns(chapters.ToAsyncEnumerable());

        _translationProvider
            .Find(Arg.Any<Guid>(), Language.English)
            .Returns(translation);

        // act
        var grain = await Silo.CreateGrainAsync<SummaryGrain>(key);
        var result = await grain.GetSummaries();

        // verify
        result.Should().BeEquivalentTo(chapters.Select(x => new Summary(x.Id, translation.Text, translation.Text)));
    }

    [Theory, AutoData]
    public async Task GivenGetSummariesCall_WhenMissingTranslation_ThenThrowException(SummaryCollectionKey key, Chapter[] chapters)
    {
        // setup
        key = key with { LangId = new LangKey(Language.English, Language.Bengali) };

        _chapterRepository
            .FindByLevel(key.Level)
            .Returns(chapters.ToAsyncEnumerable());

        // act
        var grain = await Silo.CreateGrainAsync<SummaryGrain>(key);
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await grain.GetSummaries());

        // verify
        exception.Should().NotBeNull();
    }
}

