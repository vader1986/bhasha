using System;
using System.Collections.Generic;
using Bhasha.Services;
using Bhasha.Shared.Domain.Interfaces;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public class ChapterSummariesProviderScenario
{
    public Dictionary<string, Guid> ExpressionIds => new()
    {
        ["Name"] = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        ["Description"] = Guid.Parse("11111111-1111-1111-1111-111111111112"),
        ["FirstPage"] = Guid.Parse("11111111-1111-1111-1111-111111111113"),
        ["SecondPage"] = Guid.Parse("11111111-1111-1111-1111-111111111114")
    };
        
    public IChapterRepository ChapterRepository { get; } = Substitute.For<IChapterRepository>();
    public ITranslationRepository TranslationRepository { get; } = Substitute.For<ITranslationRepository>();

    public ChapterSummariesProvider Sut { get; }

    public ChapterSummariesProviderScenario()
    {
        Sut = new ChapterSummariesProvider(ChapterRepository, TranslationRepository);
    }
}