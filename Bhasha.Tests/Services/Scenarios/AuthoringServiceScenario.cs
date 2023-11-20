using System;
using Bhasha.Services;
using Bhasha.Shared.Domain.Interfaces;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public class AuthoringServiceScenario
{
    public readonly Guid ExpressionId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    
    public IChapterRepository ChapterRepository { get; } = Substitute.For<IChapterRepository>();
    public IExpressionRepository ExpressionRepository { get; } = Substitute.For<IExpressionRepository>();
    public ITranslationRepository TranslationRepository { get; } = Substitute.For<ITranslationRepository>();
    
    public AuthoringService Sut { get; }

    public AuthoringServiceScenario()
    {
        Sut = new AuthoringService(ChapterRepository, ExpressionRepository, TranslationRepository);
    }
}