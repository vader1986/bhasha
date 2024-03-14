using System.Collections.Generic;
using Bhasha.Domain.Interfaces;
using Bhasha.Services;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public class ChapterSummariesProviderScenario
{
    public Dictionary<string, int> ExpressionIds => new()
    {
        ["Name"] = 1,
        ["Description"] = 2,
        ["FirstPage"] = 3,
        ["SecondPage"] = 4
    };
        
    public IChapterRepository ChapterRepository { get; } = Substitute.For<IChapterRepository>();
    public ITranslationProvider TranslationProvider { get; } = Substitute.For<ITranslationProvider>();

    public ChapterSummariesProvider Sut { get; }

    public ChapterSummariesProviderScenario()
    {
        Sut = new ChapterSummariesProvider(ChapterRepository, TranslationProvider);
    }
}