using Bhasha.Services;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public class AuthoringServiceScenario
{
    public Expression Expression => Expression.Create() with {Id = 1};
    
    public IChapterRepository ChapterRepository { get; } = Substitute.For<IChapterRepository>();
    public ITranslationRepository TranslationRepository { get; } = Substitute.For<ITranslationRepository>();
    public IExpressionRepository ExpressionRepository { get; } = Substitute.For<IExpressionRepository>();
    
    public AuthoringService Sut { get; }

    public AuthoringServiceScenario()
    {
        Sut = new AuthoringService(ChapterRepository, TranslationRepository, ExpressionRepository);
    }
}