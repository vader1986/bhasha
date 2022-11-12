using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Grains;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using FluentAssertions;
using NSubstitute;
using Orleans.TestKit;
using Xunit;

namespace Bhasha.Web.Tests.Grains;

public class SummaryGrainTests : TestKitBase
{
    private readonly IRepository<Chapter> _chapterRepository;
    private readonly ITranslationProvider _translationProvider;

    public SummaryGrainTests()
    {
        _chapterRepository = Substitute.For<IRepository<Chapter>>();
        _translationProvider = Substitute.For<ITranslationProvider>();

        Silo.AddService(_chapterRepository);
        Silo.AddService(_translationProvider);
    }

    [Theory, AutoData]
    public async Task GivenChaptersAndTranslations_WhenGetSummaries_ThenReturnSummaries(Chapter[] chapters, Translation translation)
    {
        // setup
        var word = translation.Native;

        var expectedChapterIds = chapters
            .Select(chapter => chapter.Id);

        var expectedSummaries = expectedChapterIds
            .Select(chapterId => new Summary(chapterId, word, word));

        var translationIds = chapters
            .Select(chapter => chapter.DescriptionId)
            .Concat(chapters.Select(chapter => chapter.NameId))
            .Distinct().ToArray();

        var translationMap = new Dictionary<Guid, Translation>();
        foreach (var translatioId in translationIds)
        {
            translationMap[translatioId] = translation;
        }

        _chapterRepository
            .Find(default)
            .ReturnsForAnyArgs(chapters);

        _translationProvider
            .FindAll(Language.English, translationIds)
            .Returns(translationMap);

        // act
        var grain = await Silo.CreateGrainAsync<SummaryGrain>($"2-{Language.English}");
        var result = await grain.GetSummaries();

        // verify
        result
            .Should()
            .BeEquivalentTo(expectedSummaries);
    }
}

