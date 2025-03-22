using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Services;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public class AuthoringServiceScenario
{
    public Expression Expression => Expression.Create() with {Id = 1};
    
    public IChapterRepository ChapterRepository { get; } = Substitute.For<IChapterRepository>();
    public ITranslationRepository TranslationRepository { get; } = Substitute.For<ITranslationRepository>();
    public ITranslationProvider TranslationProvider { get; } = Substitute.For<ITranslationProvider>();
    public IExpressionRepository ExpressionRepository { get; } = Substitute.For<IExpressionRepository>();
    
    public AuthoringService Sut { get; }

    public AuthoringServiceScenario()
    {
        Sut = new AuthoringService(ChapterRepository, TranslationRepository, TranslationProvider, ExpressionRepository);
    }
}